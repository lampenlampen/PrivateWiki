using System.IO;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace PrivateWiki.Services.SqliteStorage
{
	public class SqliteDatabase : ISqliteStorage
	{
		private readonly string _path;

		public SqliteDatabase(ISqliteStorageOptions options)
		{
			_path = options.Path;
		}

		public async Task<T> ExecuteReaderAsync<T>(SqliteCommand command, ISqliteReaderConverter<T> converter)
		{
			using var db = new SqliteConnection($"Filename={_path}");

			command.Connection = db;

			await db.OpenAsync();
			var result = await command.ExecuteReaderAsync().ConfigureAwait(false);
			db.Close();

			command.Connection = null;

			var data = converter.Convert(result);

			return data;
		}

		public async Task ExecuteNonQueryAsync(SqliteCommand command)
		{
			using var db = new SqliteConnection($"Filename={_path}");

			command.Connection = db;

			await db.OpenAsync();
			await command.ExecuteNonQueryAsync();
			db.Close();

			command.Connection = null;
		}

		public async Task<object?> ExecuteScalarAsync(SqliteCommand command)
		{
			using var db = new SqliteConnection($"Filename={_path}");

			command.Connection = db;

			await db.OpenAsync();
			var result = await command.ExecuteScalarAsync();
			db.Close();

			command.Connection = null;

			return result;
		}

		public Task DeleteDatabase()
		{
			File.Delete(Path.GetFullPath(_path));

			return Task.CompletedTask;
		}
	}
}