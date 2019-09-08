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
			_conn = new SqliteConnection($"FILENAME={_sqLite.Filename}.db");
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

		public Page GetPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public Page GetPageAsync(WikiLink link)
		{
			throw new NotImplementedException();
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

		public bool ContainsPageAsync(WikiLink link)
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
	}
}