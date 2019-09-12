using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Storage;
using Microsoft.Data.Sqlite;
using Models.Pages;
using Models.Storage;
using NodaTime;

namespace StorageBackend.SQLite
{
	public class SqLiteBackend : ISqLiteBackend
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
		}

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

		public Task<Page> GetPageAsync(Guid id)
		{
			Page Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'markdown_pages' WHERE id = @Id",
					};
					command.Parameters.AddWithValue("@Id", id);

					var reader = command.ExecuteReader();

					var page = SqliteDataReaderToMarkdownPageConverter.Instance.ConvertToPageModel(reader);

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

		public Task<Page> GetPageAsync(string link)
		{
			Page Action()
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

					var reader = command.ExecuteReader();

					var page = SqliteDataReaderToMarkdownPageConverter.Instance.ConvertToPageModel(reader);

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

		public Task<IEnumerable<Page>> GetAllPagesAsync()
		{
			IEnumerable<Page> Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'markdown_pages'",
					};

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

		public Task<bool> DeletePageAsync(Page page)
		{
			return DeletePageAsync(page.Id);
		}

		public Task<bool> DeletePageAsync(Guid id)
		{
			// TODO history

			bool Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText =
							"UPDATE 'markdown_pages_history' SET valid_to = @Now WHERE id = @Id AND valid_to = NULL;" +
							"INSERT INTO 'markdown_pages_history' (id, link, content, created, changed, locked, valid_to, valid_from, deleted) SELECT id, link, content, created, changed, locked, @Now, null, true FROM 'markdown_pages' WHERE id = @Id;" +
							"DELETE FROM 'markdown_pages' WHERE id = @Id"
					};
					command.Parameters.AddWithValue("@Id", id.ToString());
					command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());

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

		public Task<bool> InsertPageAsync(Page page)
		{
			// TODO Markdown Page
			
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
							"INSERT INTO 'markdown_pages' (id, link, content, created, changed, locked) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked)"
					};
					command.Parameters.AddWithValue("@Id", mPage.Id.ToString());
					command.Parameters.AddWithValue("@Link", mPage.Link);
					command.Parameters.AddWithValue("@Content", mPage.Content);
					command.Parameters.AddWithValue("@Created", mPage.Created.ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Changed", mPage.LastChanged.ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Locked", mPage.IsLocked);

					var result = command.ExecuteNonQuery();

					return true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return page switch
			{
				MarkdownPage mPage => new TaskFactory<bool>().StartNew((page) => MarkdownPageAction((MarkdownPage) page), mPage),
				_ => Task.FromResult(false)
			};
		}

		public Task<bool> UpdatePage(Page page)
		{
			bool MarkdownPageAction(MarkdownPage page)
			{
				using var conn = _conn;
				
				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = 
						"UPDATE 'markdown_pages_history' SET valid_to = @Now WHERE id = @Id AND valid_to = NULL;" +
						"INSERT INTO 'markdown_pages_history' (id, link, content, created, changed, locked, valid_to, valid_from, deleted) SELECT id, link, content, created, changed, locked, @Now, null, true FROM 'markdown_pages' WHERE id = @Id;" +
						"UPDATE 'markdown_pages' SET id = @Id, link = @Link, content = @Content, "
				};
			}
		}

		public bool ContainsPageAsync(Page page)
		{
			throw new NotImplementedException();
		}

		public bool ContainsPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public bool ContainsPageAsync(string link)
		{
			throw new NotImplementedException();
		}

		public Task<int> CreateTablesAsync()
		{
			using var conn = _conn;

			conn.Open();

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
				              id TEXT PRIMARY KEY, 
				              link Text UNIQUE,
							  content TEXT,
							  created INTEGER,
							  changed INTEGER,
							  locked INTEGER,
							  valid_from INTEGER,
							  valid_to INTEGER,
							  deleted INTEGER
				              );"
			};

			var task = command.ExecuteNonQueryAsync();

			return task;
		}

		public bool Delete()
		{
			throw new NotImplementedException();
		}
	}
}