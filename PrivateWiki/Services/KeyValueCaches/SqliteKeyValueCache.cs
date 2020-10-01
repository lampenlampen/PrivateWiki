using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using FluentResults;
using PrivateWiki.Services.SerializationService;
using PrivateWiki.Services.StorageServices.Sql;

namespace PrivateWiki.Services.KeyValueCaches
{
	public class SqliteKeyValueCache : IPersistentKeyValueCache
	{
		private readonly StorageServices.Sql.Sqlite.ISqliteStorage _db;
		private readonly Task _initTask;

		private const string TableName = "settings";


		public SqliteKeyValueCache(StorageServices.Sql.Sqlite.ISqliteStorage db)
		{
			_db = db;
			_initTask = InitializeDatabase();
		}

		private async Task InitializeDatabase()
		{
			var sql = "CREATE TABLE IF NOT EXISTS settings (key Text PRIMARY KEY, value TEXT)";

			var parameter = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@TableName", TableName)
			};

			var command = new SqlCommand(sql, parameter);

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

			var sql = "INSERT INTO settings VALUES (@Key, @Value) ON CONFLICT (key) Do UPDATE  SET value = @Value;";
			var parameter = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@TableName", TableName),
				new KeyValuePair<string, string>("@Key", key),
				new KeyValuePair<string, string>("@Value", value)
			};

			var command = new SqlCommand(sql, parameter);

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

		public async Task<Result> InsertAsync<T>(string key, T value, IJsonSerializationService<T> serializer) => Result.Ok(InsertAsync(key, await serializer.Serialize(value)));

		public async Task<Result<T>> GetObjectAsync<T>(string key, IJsonSerializationService<T> deserializer)
		{
			await _initTask;

			var data = (string?) await Get(key).ConfigureAwait(false);

			if (data is null) return Result.Fail(new KeyNotFoundError(key));

			var result = await deserializer.Deserialize(data);

			if (result.IsFailed) return result;

			return Result.Ok(result.Value);
		}

		public async Task<Result> RemoveAsync(string key)
		{
			await _initTask;

			var sql = "DELETE FROM settings WHERE key == @Key";
			var parameters = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@Key", key)
			};

			var command = new SqlCommand(sql, parameters);

			await _db.ExecuteNonQueryAsync(command).ConfigureAwait(false);

			return Result.Ok();
		}

		public async Task<Result> RemoveAllAsync()
		{
			await _initTask;

			var sql = "DELETE FROM settings";
			var parameters = new List<KeyValuePair<string, string>>();

			var command = new SqlCommand(sql, parameters);

			await _db.ExecuteNonQueryAsync(command).ConfigureAwait(false);

			return Result.Ok();
		}

		public Task DeleteCache() => _db.DeleteDatabase();

		private Task<string?> Get(string key)
		{
			var sql = "SELECT value FROM settings WHERE key == @Key";
			var parameters = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@TableName", TableName),
				new KeyValuePair<string, string>("@Key", key)
			};

			var command = new SqlCommand(sql, parameters);

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