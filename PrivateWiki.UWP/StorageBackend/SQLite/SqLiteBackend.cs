using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Utilities;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;

namespace PrivateWiki.UWP.StorageBackend.SQLite
{
	#nullable enable

	public class SqLiteBackend : ISqLiteBackend, IMarkdownPageStorage, IPageStorageBackendServiceImpl
	{
		private const bool IsObsoleteError = false;

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
				    );
				    Create TABLE IF NOT EXISTS labels (
				        id TEXT PRIMARY KEY ,
				        key TEXT,
				        value TEXT,
				        description TEXT,
				        color TEXT
				    );
				    CREATE TABLE IF NOT EXISTS page_labels (
				        page_id TEXT,
				        label_id TEXT,
				        PRIMARY KEY (page_id, label_id),
				        FOREIGN KEY (page_id) REFERENCES pages(id) on delete CASCADE,
				        FOREIGN KEY (label_id) REFERENCES labels(id) on delete CASCADE
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

		[Obsolete("Use generic methods", IsObsoleteError)]
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
						CommandText = "SELECT * FROM 'pages' WHERE id = @Id",
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

		[Obsolete("Use generic methods", IsObsoleteError)]
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
						CommandText = "SELECT * FROM 'pages' WHERE link = @Link",
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

		[Obsolete("Use generic methods", IsObsoleteError)]
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
						CommandText = "SELECT * FROM 'pages'",
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

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<bool> UpdateMarkdownPage(MarkdownPage page, PageAction action)
		{
			bool MarkdownPageAction(MarkdownPage page)
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
				command.Parameters.AddWithValue("@ContentType", page.ContentType);
				command.Parameters.AddWithValue("@Now", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@Action", action);

				conn.Open();
				var result = command.ExecuteNonQuery();

				return true;
			}

			return new TaskFactory<bool>().StartNew((page) => MarkdownPageAction((MarkdownPage) page), page);
		}

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<bool> DeleteMarkdownPageAsync(MarkdownPage page)
		{
			return DeleteMarkdownPageAsync(page.Id);
		}

		[Obsolete("Use generic methods", IsObsoleteError)]
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

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<bool> InsertMarkdownPageAsync(MarkdownPage page)
		{
			bool MarkdownPageAction(MarkdownPage page)
			{
				var mPage = (MarkdownPage) page;

				var conn = _conn;

				try
				{
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText =
							"INSERT INTO 'pages' (id, link, content, created, changed, locked, contentType) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, @ContentType);\nINSERT INTO 'pages_history' (id, link, content, created, changed, locked, contentType, valid_to, valid_from, action) VALUES (@Id, @Link, @Content, @Created, @Changed, @Locked, @ContentType, -1, @Now, @Action);"
					};
					command.Parameters.AddWithValue("@Id", mPage.Id.ToString());
					command.Parameters.AddWithValue("@Link", mPage.Link);
					command.Parameters.AddWithValue("@Content", mPage.Content);
					command.Parameters.AddWithValue("@Created", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Changed", _clock.GetCurrentInstant().ToUnixTimeMilliseconds());
					command.Parameters.AddWithValue("@Locked", mPage.IsLocked);
					command.Parameters.AddWithValue("@ContentType", page.ContentType);
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

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<bool> ContainsMarkdownPageAsync(MarkdownPage page)
		{
			bool MarkdownPageAction(MarkdownPage page)
			{
				using var conn = _conn;

				var command = new SqliteCommand
				{
					Connection = conn,
					CommandText = "SELECT * FROM 'pages' WHERE id = @Id"
				};
				command.Parameters.AddWithValue("@Id", page.Id.ToString());

				conn.Open();
				var result = command.ExecuteNonQuery();

				return result == 1;
			}

			return new TaskFactory<bool>().StartNew(page => MarkdownPageAction((MarkdownPage) page), page);
		}

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<bool> ContainsMarkdownPageAsync(Guid id)
		{
			bool MarkdownPageAction(Guid id)
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

			return new TaskFactory<bool>().StartNew(id => MarkdownPageAction((Guid) id), id);
		}

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<bool> ContainsMarkdownPageAsync(string link)
		{
			bool MarkdownPageAction(string link)
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

			return new TaskFactory<bool>().StartNew(link => MarkdownPageAction((string) link), link);
		}

		[Obsolete("Use generic methods", IsObsoleteError)]
		public Task<IEnumerable<PageHistory<MarkdownPage>>> GetMarkdownPageHistoryAsync(string pageLink)
		{
			IEnumerable<PageHistory<MarkdownPage>> Action(string pageLink)
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
				var pages = SqliteDataReaderToHistoryMarkdownPageConverter.Instance.ConvertToHistoryMarkdownPageModels(result);

				return pages;
			}

			return new TaskFactory<IEnumerable<PageHistory<MarkdownPage>>>().StartNew(page => Action((string) page), pageLink);
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

		public Task<bool> InsertLabelAsync(Label label)
		{
			bool Action(Label label)
			{
				try
				{
					using var conn = _conn;

					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "INSERT INTO 'labels' (id, key, value, description, color) VALUES (@id, @key, @value, @description, @color)"
					};

					command.Parameters.AddWithValue("@id", label.Id.ToString());
					command.Parameters.AddWithValue("@key", label.Key);
					command.Parameters.AddWithValue("@value", label.Value);
					command.Parameters.AddWithValue("@description", label.Description);
					command.Parameters.AddWithValue("@color", label.Color.ToHexColor());

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

			return new TaskFactory<bool>().StartNew((label) => Action((Label) label), label);
		}

		public Task<Label> GetLabelAsync(Guid id)
		{
			Label Action(Guid id)
			{
				try
				{
					using var conn = _conn;
					
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM labels WHERE id == @id"
					};

					command.Parameters.AddWithValue("@id", id.ToString());
					
					conn.Open();

					var reader = command.ExecuteReader();
					
					id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
					var key = reader.GetString(reader.GetOrdinal("key"));
					var value = reader.GetString(reader.GetOrdinal("value"));
					var description = reader.GetString(reader.GetOrdinal("description"));
					var color = reader.GetString(reader.GetOrdinal("color")).HexToColor();
					
					var label = new Label(id, key, value, description, color);

					return label;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
			
			return new TaskFactory<Label>().StartNew((id) => Action((Guid) id), id);
		}

		public Task<IEnumerable<Label>> GetAllLabelsAsync()
		{
			IEnumerable<Label> Action()
			{
				try
				{
					using var conn = _conn;
					
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT * FROM labels"
					};

					conn.Open();

					var reader = command.ExecuteReader();
					
					var labels = new List<Label>();

					while (reader.Read())
					{
						var id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
						var key = reader.GetString(reader.GetOrdinal("key"));
						var value = reader.GetString(reader.GetOrdinal("value"));
						var description = reader.GetString(reader.GetOrdinal("description"));
						var color = reader.GetString(reader.GetOrdinal("color")).HexToColor();
					
						var label = new Label(id, key, value, description, color);
						
						labels.Add(label);
					}
					
					return labels;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
			
			return new TaskFactory<IEnumerable<Label>>().StartNew(Action);
		}

		public Task<bool> DeleteLabelAsync(Guid id)
		{
			bool Action(Guid id)
			{
				try
				{
					using var conn = _conn;
					
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "DELETE FROM labels WHERE id == @id"
					};

					command.Parameters.AddWithValue("@id", id.ToString());
					
					conn.Open();

					command.ExecuteNonQuery();

					return true;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
			
			return new TaskFactory<bool>().StartNew((id) => Action((Guid) id), id);
		}

		public Task<bool> DeleteLabelAsync(Label label) => DeleteLabelAsync(label.Id);

		public Task<IEnumerable<Label>> GetLabelsForPage(Guid pageId)
		{
			IEnumerable<Label> Action(Guid pageId)
			{
				try
				{
					using var conn = _conn;
					
					var command = new SqliteCommand
					{
						Connection = conn,
						CommandText = "SELECT l.id, l.key, l.value, l.description, l.color\nFROM page_labels a, labels l\nWHERE a.label_id == l.id AND a.page_id == @id"
					};

					command.Parameters.AddWithValue("@id", pageId.ToString());

					var reader = command.ExecuteReader();

					var labels = new List<Label>();
					
					while (reader.Read())
					{
						var id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
						var key = reader.GetString(reader.GetOrdinal("key"));
						var value = reader.GetString(reader.GetOrdinal("value"));
						var description = reader.GetString(reader.GetOrdinal("description"));
						var color = reader.GetString(reader.GetOrdinal("color")).HexToColor();
						
						var label = new Label(id, key, value, description, color);
						
						labels.Add(label);
					}

					return labels;
				}
				catch (Exception e)
				{
					Console.WriteLine(e);
					throw;
				}
			}
			
			return new TaskFactory<IEnumerable<Label>>().StartNew((id) => Action((Guid) id), pageId);
		}

		#endregion
	}
}