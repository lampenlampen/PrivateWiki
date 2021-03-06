using System;
using System.Data.Common;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PrivateWiki.Core;

namespace PrivateWiki.Services.StorageServices.Sql.Sqlite
{
	public class SqliteDatabase : ISqliteStorage
	{
		private readonly SqliteStorageOptions _options;
		private readonly string _connString;

		public SqliteDatabase(SqliteStorageOptions options)
		{
			_options = options;
			_connString = new SqliteConnectionStringBuilder
			{
				DataSource = _options.Path,
				Mode = SqliteOpenMode.ReadWriteCreate
			}.ToString();
		}

		public async Task<T> ExecuteReaderAsync<T>(SqlCommand command, IConverter<DbDataReader, T> converter)
		{
			using var db = new SqliteConnection(_connString);

			var sqliteCommand = new SqliteCommand
			{
				Connection = db,
				CommandText = command.Sql,
			};

			foreach (var pair in command.Parameters)
			{
				sqliteCommand.Parameters.AddWithValue(pair.Key, pair.Value);
			}

			await db.OpenAsync().ConfigureAwait(false);
			var result = await sqliteCommand.ExecuteReaderAsync().ConfigureAwait(false);

			var data = converter.Convert(result);

			return data;
		}

		public async Task ExecuteNonQueryAsync(SqlCommand command)
		{
			using var db = new SqliteConnection(_connString);

			var sqliteCommand = new SqliteCommand
			{
				Connection = db,
				CommandText = command.Sql
			};

			foreach (var pair in command.Parameters)
			{
				sqliteCommand.Parameters.AddWithValue(pair.Key, pair.Value);
			}

			await db.OpenAsync().ConfigureAwait(false);
			await sqliteCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
		}

		public async Task<string?> ExecuteScalarAsync(SqlCommand command)
		{
			using var db = new SqliteConnection(_connString);

			var sqliteCommand = new SqliteCommand
			{
				Connection = db,
				CommandText = command.Sql
			};

			foreach (var pair in command.Parameters)
			{
				sqliteCommand.Parameters.AddWithValue(pair.Key, pair.Value);
			}

			await db.OpenAsync().ConfigureAwait(false);
			var result = await sqliteCommand.ExecuteScalarAsync().ConfigureAwait(false);

			return result is null || result is DBNull ? null : result.ToString();
		}

		public Task DeleteDatabase()
		{
			File.Delete(Path.GetFullPath(_options.Path));

			return Task.CompletedTask;
		}
	}
}