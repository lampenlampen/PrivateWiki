using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace PrivateWiki.Services.SqliteStorage
{
	public interface ISqliteStorage
	{
		public Task<T> ExecuteReaderAsync<T>(SqliteCommand command, ISqliteReaderConverter<T> converter);

		public Task ExecuteNonQueryAsync(SqliteCommand command);

		public Task<object?> ExecuteScalarAsync(SqliteCommand command);

		public Task DeleteDatabase();
	}
}