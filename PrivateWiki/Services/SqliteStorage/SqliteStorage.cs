using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace PrivateWiki.Services.SqliteStorage
{
	public interface ISqliteStorage
	{
		public Task<T> ExecuteReaderAsync<T>(SqliteCommand command, ISqliteReaderConverter<T> converter);

		public Task ExecuteNonQueryAsync(SqliteCommand command);

		public Task<object?> ExecuteScalarAsync(SqliteCommand command);
	}

	public interface ISqliteStorageOptions
	{
		string Path { get; set; }
	}

	public class SqliteStorageOptions : ISqliteStorageOptions
	{
		public string Path { get; set; }
	}

	public interface ISqliteReaderConverter<out T>
	{
		T Convert(SqliteDataReader reader);
	}
	
	public class SqliteReaderConverter : ISqliteReaderConverter<string>
	{
		public string Convert(SqliteDataReader reader)
		{
			return "";
		}
	}
}