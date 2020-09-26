using System.Threading.Tasks;

namespace PrivateWiki.Services.SqliteStorage.v2
{
	public interface ISqliteStorage
	{
		public Task<T> ExecuteReaderAsync<T>(SqliteCommand command, ISqliteReaderConverter<T> converter);

		public Task ExecuteNonQueryAsync(SqliteCommand command);

		public Task<string?> ExecuteScalarAsync(SqliteCommand command);

		public Task DeleteDatabase();
	}
}