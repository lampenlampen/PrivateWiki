using Microsoft.Data.Sqlite;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.SqliteStorage
{
	public interface ISqliteReaderConverter<out TResult>
	{
		TResult Convert<TInput>(TInput reader);
	}
}