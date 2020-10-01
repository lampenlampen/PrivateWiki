using System.Data.Common;
using System.Threading.Tasks;
using PrivateWiki.Core;

namespace PrivateWiki.Services.StorageServices.Sql.Sqlite
{
	public interface ISqliteStorage
	{
		public Task<T> ExecuteReaderAsync<T>(SqlCommand command, IConverter<DbDataReader, T> converter);

		public Task ExecuteNonQueryAsync(SqlCommand command);

		public Task<string?> ExecuteScalarAsync(SqlCommand command);

		public Task DeleteDatabase();
	}
}