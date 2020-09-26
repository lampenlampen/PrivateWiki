using Microsoft.Data.Sqlite;

namespace PrivateWiki.Services.SqliteStorage
{
	public interface ISqliteReaderConverter<out T>
	{
		T Convert(SqliteDataReader reader);
	}
}