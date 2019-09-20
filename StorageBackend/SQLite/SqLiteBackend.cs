using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Storage;
using Microsoft.Data.Sqlite;
using Models.Pages;
using Models.Storage;
using NodaTime;

namespace StorageBackend.SQLite
{
	public class SqLiteBackend : ISqLiteBackend, IMarkdownPageStorage
	{
		private readonly SqLiteStorage _sqLite;

		private SqliteConnection _conn;

		public SqliteConnection Connection => _conn;

		private IClock _clock;

		public SqLiteBackend(SqLiteStorage sqLite, IClock clock)
		{
			_sqLite = sqLite;
			_clock = clock;

			var connString = new SqliteConnectionStringBuilder
			{
				DataSource = $"{_sqLite.Filename}.db",
				Mode = SqliteOpenMode.ReadWriteCreate
			};
			_conn = new SqliteConnection(connString.ToString());

			CreateTablesAsync().Wait();
		}

		#region ISqLiteBackend Members
		
		public Task<bool> ExistsAsync()
		{
			bool Action()
			{
				var sqliteConnectionString = new SqliteConnectionStringBuilder
					{Mode = SqliteOpenMode.ReadOnly, DataSource = $"{_sqLite.Filename}.db"};

				try
				{
					using var conn = new SqliteConnection(sqliteConnectionString.ToString());
					conn.Open();
				}
				catch (SqliteException e)
				{
					if (e.SqliteErrorCode == 14) return false;
				}

				return true;
			}

			return Task.Run(Action);
		}

		public Task<int> CreateTablesAsync()
		{
			using var conn = _conn;

			var command = new SqliteCommand
			{
				Connection = conn,
				CommandText = @"CREATE TABLE IF NOT EXISTS markdown_pages (
				              id TEXT PRIMARY KEY, 
				              link Text UNIQUE,
							  content TEXT,
							  created INTEGER,
							  changed INTEGER,
							  locked INTEGER
				              );
				              CREATE TABLE IF NOT EXISTS markdown_pages_history (
				              id TEXT, 
				              link Text,
							  content TEXT,
							  created INTEGER,
							  changed INTEGER,
							  locked INTEGER,
							  valid_from INTEGER,
							  valid_to INTEGER,
							  action Integer,
							  PRIMARY KEY (id, valid_from, valid_to)
				              );"
			};

			conn.Open();
			var task = command.ExecuteNonQueryAsync();

			return task;
		}

		public bool Delete()
		{
			throw new NotImplementedException();
		}
		
		#endregion

		#region IMarkdownPageStorage members

		public Task<MarkdownPage> GetMarkdownPageAsync(Guid id)
		{
			MarkdownPage Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'markdown_pages' WHERE id = @Id",
					};
					command.Parameters.AddWithValue("@Id", id.ToString());

					conn.Open();
					var reader = command.ExecuteReader();

					var page = new MarkdownPage();
					SqliteDataReaderToMarkdownPageConverter.Instance.ConvertToMarkdownPageModel(reader, page);

					return page;
				}
				catch (SqliteException e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return Task.Run(Action);
		}

		public Task<MarkdownPage> GetMarkdownPageAsync(string link)
		{
			MarkdownPage Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'markdown_pages' WHERE link = @Link",
					};
					command.Parameters.AddWithValue("@Link", link);

					conn.Open();
					var reader = command.ExecuteReader();

					var page = new MarkdownPage();
					SqliteDataReaderToMarkdownPageConverter.Instance.ConvertToMarkdownPageModel(reader, page);

					return page;
				}
				catch (SqliteException e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return Task.Run(Action);
		}

		public Task<IEnumerable<MarkdownPage>> GetAllMarkdownPagesAsync()
		{
			IEnumerable<MarkdownPage> Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'markdown_pages'",
					};

					conn.Open();
					var reader = command.ExecuteReader();

					var pages = SqliteDataReaderToMarkdownPageConverter.Instance.ConvertToPageModels(reader);

					return pages;
				}
				catch (SqliteException e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return Task.Run(Action);
		}

		public Task<bool> UpdateMarkdownPage(MarkdownPage page, PageAction action)
		{
			bool MarkdownPageAction(MarkdownPage page)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText =
						"UPDATE 'markdown_pages_history' SET valid_to = @Now WHERE valid_to = -1;\nINSERT INTO 'markdown_pages_history' (id, link, content, created, changed, locked, valid_to, valid_from, action) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, @Now, -1, @Action);\nUPDATE 'markdown_pages' SET id = @Id, link = @Link, content = @Content, created = @Created, changed = @Changed, locked = @Locked WHERE id = @Id;"
				};
				command.Parameters.AddWithValue("@Id", page.Id.ToString());
				command.Parameters.AddWithValue("@Link", page.Link);
				command.Parameters.AddWithValue("@Content", page.Content);
				command.Parameters.AddWithValue("@Created", page.Created.ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Changed", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Locked", page.IsLocked);
				command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Action", action);

				conn.Open();
				var result = command.ExecuteNonQuery();

				return true;
			}

			return new TaskFactory<bool>().StartNew((page) => MarkdownPageAction((MarkdownPage) page), page);
		}

		public Task<bool> DeleteMarkdownPageAsync(MarkdownPage page)
		{
			return DeleteMarkdownPageAsync(page.Id);
		}

		public Task<bool> DeleteMarkdownPageAsync(Guid id)
		{
			bool Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText =
							"UPDATE 'markdown_pages_history' SET valid_to = @Now WHERE valid_to = -1;\nINSERT INTO 'markdown_pages_history' (id, link, content, created, changed, locked, valid_to, valid_from, action) SELECT id, link, content, created, changed, locked, -1, @Now, @Action FROM 'markdown_pages' WHERE id = @Id;\nDELETE FROM 'markdown_pages' WHERE id = @Id"
					};
					command.Parameters.AddWithValue("@Id", id.ToString());
					command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Action", PageAction.Deleted.ToString());

					conn.Open();
					var result = command.ExecuteNonQuery();

					var a = result == 1;

					return true;
				}
				catch (SqliteException e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return Task.Run(Action);
		}

		public Task<bool> InsertMarkdownPageAsync(MarkdownPage page)
		{
			bool MarkdownPageAction(MarkdownPage page)
			{
				var mPage = (MarkdownPage) page;

				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText =
							"INSERT INTO 'markdown_pages' (id, link, content, created, changed, locked) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked);\nINSERT INTO 'markdown_pages_history' (id, link, content, created, changed, locked, valid_to, valid_from, action) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, -1, @Now, @Action);"
					};
					command.Parameters.AddWithValue("@Id", mPage.Id.ToString());
					command.Parameters.AddWithValue("@Link", mPage.Link);
					command.Parameters.AddWithValue("@Content", mPage.Content);
					command.Parameters.AddWithValue("@Created", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Changed", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Locked", mPage.IsLocked);
					command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Action", PageAction.Created);

					conn.Open();
					var result = command.ExecuteNonQuery();

					return true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return new TaskFactory<bool>().StartNew((page) => MarkdownPageAction((MarkdownPage) page), page);
		}

		public Task<bool> ContainsMarkdownPageAsync(MarkdownPage page)
		{
			bool MarkdownPageAction(MarkdownPage page)
			{
				using var conn = _conn;
				
				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'markdown_pages' WHERE id = @Id"
				};
				command.Parameters.AddWithValue("@Id", page.Id.ToString());

				conn.Open();
				var result = command.ExecuteNonQuery();

				return result == 1;
			}

			return new TaskFactory<bool>().StartNew(page => MarkdownPageAction((MarkdownPage) page), page);
		}

		public Task<bool> ContainsMarkdownPageAsync(Guid id)
		{
			bool MarkdownPageAction(Guid id)
			{
				using var conn = _conn;
				
				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'markdown_pages' WHERE id = @Id"
				};
				command.Parameters.AddWithValue("@Id", id.ToString());

				conn.Open();
				var result = command.ExecuteNonQuery();

				return result == 1;
			}

			return new TaskFactory<bool>().StartNew(id => MarkdownPageAction((Guid) id), id);
		}

		public Task<bool> ContainsMarkdownPageAsync(string link)
		{
			bool MarkdownPageAction(string link)
			{
				using var conn = _conn;
				
				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'markdown_pages' WHERE link = @Link"
				};
				command.Parameters.AddWithValue("@Link", link);

				conn.Open();
				var result = command.ExecuteScalar();

				return result != null;
			}

			return new TaskFactory<bool>().StartNew(link => MarkdownPageAction((string) link), link);
		}

		public Task<IEnumerable<PageHistory<MarkdownPage>>> GetMarkdownPageHistoryAsync(string pageLink)
		{
			IEnumerable<PageHistory<MarkdownPage>> Action(string pageLink)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'markdown_pages_history' WHERE link = @Link ORDER BY valid_from"
				};
				command.Parameters.AddWithValue("@Link", pageLink);

				conn.Open();
				var result = command.ExecuteReader();
				var pages = SqliteDataReaderToHistoryMarkdownPageConverter.Instance.ConvertToHistoryMarkdownPageModels(result);

				return pages;
			}

			return new TaskFactory<IEnumerable<PageHistory<MarkdownPage>>>().StartNew(page => Action((string) page), pageLink);
		}

		#endregion
	}
}