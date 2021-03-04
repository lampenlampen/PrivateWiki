using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NodaTime;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.StorageBackendService.SQLite
{
	public class SqLiteBackend : ISqLiteBackend, IPageStorageBackendServiceImpl
	{
		private readonly SqLiteStorageOptions _sqLite;

		private readonly SqliteConnection _conn;

		public SqliteConnection Connection => _conn;

		private readonly IClock _clock;

		public SqLiteBackend(SqLiteStorageOptions sqLite, IClock clock)
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
					// Indicates that SqLite can't open the db.
					// https://sqlite.org/rescode.html#cantopen
					if (e.SqliteErrorCode == 14) return false;
				}

				return true;
			}

			return Task.Run(Action);
		}

		public Task<int[]> CreateTablesAsync()
		{
			return Task.WhenAll(
				CreateMarkdownTablesAsync());
		}

		private Task<int> CreateMarkdownTablesAsync()
		{
			using var conn = _conn;

			var command = new SqliteCommand
			{
				Connection = conn,
				CommandText =
					@"CREATE TABLE IF NOT EXISTS pages (
				    id TEXT PRIMARY KEY, 
				    link Text UNIQUE,
					content TEXT,
					created INTEGER,
					changed INTEGER,
					locked INTEGER,
					contentType TEXT
				    );
				    CREATE TABLE IF NOT EXISTS pages_history (
				    id TEXT, 
				    link Text,
					content TEXT,
					created INTEGER,
					changed INTEGER,
					locked INTEGER,
					contentType TEXT,
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

		#region IGenericPageStorage Members

		public Task<GenericPage> GetPageAsync(Guid id)
		{
			GenericPage Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'pages' WHERE id = @Id",
					};
					command.Parameters.AddWithValue("@Id", id.ToString());

					conn.Open();
					var reader = command.ExecuteReader();

					var page = new GenericPage();
					SqliteDataReaderToPageConverter.Instance.ConvertToPageModel(reader, page);

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

		public Task<GenericPage> GetPageAsync(string link)
		{
			GenericPage Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'pages' WHERE link = @Link",
					};
					command.Parameters.AddWithValue("@Link", link);

					conn.Open();
					var reader = command.ExecuteReader();

					var page = new GenericPage();
					SqliteDataReaderToPageConverter.Instance.ConvertToPageModel(reader, page);

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

		public Task<IEnumerable<GenericPage>> GetAllPagesAsync()
		{
			IEnumerable<GenericPage> Action()
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM 'pages'",
					};

					conn.Open();
					var reader = command.ExecuteReader();

					var pages = SqliteDataReaderToPageConverter.Instance.ConvertToPageModels(reader);

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

		public Task<bool> UpdatePage(GenericPage page, PageAction action)
		{
			bool PageAction(GenericPage page)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText =
						"UPDATE 'pages_history' SET valid_to = @Now WHERE valid_to = -1 AND id = @Id;\nINSERT INTO 'pages_history' (id, link, content, created, changed, locked, contentType, valid_to, valid_from, action) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, @ContentType, -1, @Now, @Action);\nUPDATE 'pages' SET id = @Id, link = @Link, content = @Content, created = @Created, changed = @Changed, locked = @Locked, contentType = @ContentType WHERE id = @Id;"
				};
				command.Parameters.AddWithValue("@Id", page.Id.ToString());
				command.Parameters.AddWithValue("@Link", page.Link);
				command.Parameters.AddWithValue("@Content", page.Content);
				command.Parameters.AddWithValue("@Created", page.Created.ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Changed", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Locked", page.IsLocked);
				command.Parameters.AddWithValue("@ContentType", page.ContentType.Name);
				command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Action", action);

				conn.Open();
				var result = command.ExecuteNonQuery();

				return true;
			}

			return new TaskFactory<bool>().StartNew((page) => PageAction((GenericPage) page), page);
		}

		public Task<bool> DeletePageAsync(GenericPage page)
		{
			return DeletePageAsync(page.Id);
		}

		public Task<bool> DeletePageAsync(Guid id)
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
							"UPDATE 'pages_history' SET valid_to = @Now WHERE valid_to = -1 AND id = @Id;\nINSERT INTO 'pages_history' (id, link, content, created, changed, locked, contentType, valid_to, valid_from, action) SELECT id, link, content, created, changed, locked, contentType, -1, @Now, @Action FROM 'pages' WHERE id = @Id;\nDELETE FROM 'pages' WHERE id = @Id"
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

		public Task<bool> InsertPageAsync(GenericPage page)
		{
			bool PageAction(GenericPage page)
			{
				using var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText =
							"INSERT INTO 'pages' (id, link, content, created, changed, locked, contentType) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, @ContentType);\nINSERT INTO 'pages_history' (id, link, content, created, changed, locked, contentType, valid_to, valid_from, action) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, @ContentType, -1, @Now, @Action);"
					};
					command.Parameters.AddWithValue("@Id", page.Id.ToString());
					command.Parameters.AddWithValue("@Link", page.Link);
					command.Parameters.AddWithValue("@Content", page.Content);
					command.Parameters.AddWithValue("@Created", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Changed", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Locked", page.IsLocked);
					command.Parameters.AddWithValue("@ContentType", page.ContentType.Name);
					command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Action", global::PrivateWiki.DataModels.Pages.PageAction.Created);

					conn.Open();

					command.ExecuteReader();

					return true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}

			return new TaskFactory<bool>().StartNew((page) => PageAction((GenericPage) page), page);
		}

		public Task<bool> ContainsPageAsync(GenericPage page)
		{
			return ContainsPageAsync(page.Path.FullPath);
		}

		public Task<bool> ContainsPageAsync(Guid id)
		{
			bool PageAction(Guid id)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'pages' WHERE id = @Id"
				};
				command.Parameters.AddWithValue("@Id", id.ToString());

				conn.Open();
				var result = command.ExecuteNonQuery();

				return result == 1;
			}

			return new TaskFactory<bool>().StartNew(id => PageAction((Guid) id), id);
		}

		public Task<bool> ContainsPageAsync(string link)
		{
			bool PageAction(string link)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'pages' WHERE link = @Link"
				};
				command.Parameters.AddWithValue("@Link", link);

				conn.Open();
				var result = command.ExecuteScalar();

				return result != null;
			}

			return new TaskFactory<bool>().StartNew(link => PageAction((string) link), link);
		}

		public Task<IEnumerable<GenericPageHistory>> GetPageHistoryAsync(string pageLink)
		{
			IEnumerable<GenericPageHistory> Action(string pageLink)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'pages_history' WHERE link = @Link ORDER BY valid_from"
				};
				command.Parameters.AddWithValue("@Link", pageLink);

				conn.Open();
				var result = command.ExecuteReader();
				var pages = SqliteDataReaderToHistoryPageConverter.Instance.ConvertToHistoryPageModels(result);

				return pages;
			}

			return new TaskFactory<IEnumerable<GenericPageHistory>>().StartNew(page => Action((string) page), pageLink);
		}

		#endregion
	}
}