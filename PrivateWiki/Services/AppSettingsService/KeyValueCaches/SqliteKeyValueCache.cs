using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FluentResults;
using Microsoft.Data.Sqlite;
using PrivateWiki.Services.SqliteStorage;

namespace PrivateWiki.Services.AppSettingsService.KeyValueCaches
{
	public class SqliteKeyValueCache : IPersistentKeyValueCache
	{
		private readonly ISqliteStorage _db;
		private readonly Task _initTask;

		private const string TableName = "settings";


		public SqliteKeyValueCache(ISqliteStorage db)
		{
			_db = db;
			_initTask = InitializeDatabase();
		}

		private async Task InitializeDatabase()
		{
			var command = new SqliteCommand
			{
				CommandText = "CREATE TABLE IF NOT EXISTS settings (key Text PRIMARY KEY, value TEXT);"
			};

			command.Parameters.AddWithValue("@TableName", TableName);

			await _db.ExecuteNonQueryAsync(command);
		}

		public Task<Result> InsertAsync(string key, int value) => InsertAsync(key, Convert.ToString(value));

		public async Task<Result<int>> GetIntAsync(string key)
		{
			await _initTask;

			var result = (string?) await Get(key).ConfigureAwait(false);

			if (result is null) return Result.Fail(new KeyNotFoundError(key));
			
			return Result.Ok(Convert.ToInt32(result));
		}

		public async Task<Result> InsertAsync(string key, string value)
		{
			await _initTask;
			
			var command = new SqliteCommand
			{
				CommandText = "INSERT INTO settings VALUES (@Key, @Value) ON CONFLICT (key) Do UPDATE  SET value = @Value;"
			};

			command.Parameters.AddWithValue("@TableName", TableName);
			command.Parameters.AddWithValue("@Key", key);
			command.Parameters.AddWithValue("@Value", value);

			await _db.ExecuteNonQueryAsync(command).ConfigureAwait(false);
			
			return Result.Ok();
		}

		public async Task<Result<string>> GetStringAsync(string key)
		{
			await _initTask;
			
			var result = (string?) await Get(key).ConfigureAwait(false);

			if (result is null)
			{
				return Result.Fail(new KeyNotFoundError(key));
			}
			
			return Result.Ok(result);
		}

		public Task<Result> InsertAsync(string key, bool value) => InsertAsync(key, Convert.ToString(value));

		public async Task<Result<bool>> GetBooleanAsync(string key)
		{
			await _initTask;
			
			var result = (string?) await Get(key).ConfigureAwait(false);
			
			if (result is null)
			{
				return Result.Fail(new KeyNotFoundError(key));
			}
			
			return Result.Ok(Convert.ToBoolean(result));
		}

		public Task<Result> InsertAsync<T>(string key, T value) => InsertAsync(key, Serialize(value));

		public async Task<Result<T>> GetObjectAsync<T>(string key)
		{
			await _initTask;
			
			var result = (string?) await Get(key).ConfigureAwait(false);

			if (result is null) return Result.Fail(new KeyNotFoundError(key));
			
			return Result.Ok(Deserialize<T>(result));
		}

		public async Task<Result> RemoveAsync(string key)
		{
			await _initTask;
			
			var command = new SqliteCommand
			{
				CommandText = "DELETE FROM settings WHERE key == @Key"
			};

			command.Parameters.AddWithValue("@Key", key);

			await _db.ExecuteNonQueryAsync(command).ConfigureAwait(false);

			return Result.Ok();
		}

		public async Task<Result> RemoveAllAsync()
		{
			await _initTask;
			
			var command = new SqliteCommand
			{
				CommandText = "DELETE FROM settings"
			};

			await _db.ExecuteNonQueryAsync(command).ConfigureAwait(false);

			return Result.Ok();
		}

		private Task<object?> Get(string key)
		{
			var command = new SqliteCommand
			{
				CommandText = "SELECT value FROM settings WHERE key == @Key"
			};

			command.Parameters.AddWithValue("@TableName", TableName);
			// command.Parameters.AddWithValue("@ValueCol", ValueCol);
			// command.Parameters.AddWithValue("@KeyCol", KeyCol);
			command.Parameters.AddWithValue("@Key", key);

			return _db.ExecuteScalarAsync(command);
		}
		
		public string this[string key]
		{
			get
			{
				var result = (string?) Get(key).GetAwaiter().GetResult();

				if (result is null) throw new KeyNotFoundException(key);
				
				return result;
			}
			set => InsertAsync(key, value).GetAwaiter().GetResult();
		}
	
		private string Serialize<T>(T value) => JsonSerializer.Serialize(value);

		private T Deserialize<T>(string data) => JsonSerializer.Deserialize<T>(data);
	}
}