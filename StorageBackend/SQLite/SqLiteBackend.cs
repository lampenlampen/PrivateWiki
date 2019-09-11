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
				var sqliteConnectionString = new SqliteConnectionStringBuilder {Mode = SqliteOpenMode.ReadOnly, DataSource = $"{_sqLite.Filename}.db"};

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

		public Task<T> GetPageAsync<T>(Guid id) where T : Page
		{
			T Action()
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

		public Task<MarkdownPage> GetPageAsync(Guid id)
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

		public Page GetPageAsync(string link)
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

		public IEnumerable<Page> GetAllPagesAsync()
		{
			throw new NotImplementedException();
		}

		public bool DeletePageAsync(Page page)
		{
			throw new NotImplementedException();
		}

		public bool DeletePageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Guid InsertPageAsync(Page page)
		{
			throw new NotImplementedException();
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