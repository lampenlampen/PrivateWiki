using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using PrivateWiki.Services.SqliteStorage;

namespace PrivateWiki.Services.StorageServices.Sql.Sqlite
{
	public interface ISqliteStorage
	{
		public Task<T> ExecuteReaderAsync<T>(SqlCommand command, IConverter<SqliteDataReader, T> converter);

		public Task ExecuteNonQueryAsync(SqlCommand command);

		public Task<string?> ExecuteScalarAsync(SqlCommand command);

		public Task DeleteDatabase();
	}
}