using Microsoft.Data.Sqlite;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.StorageServices.Sql.Sqlite
{
	public class SqliteReaderToLabelConverter : IConverter<SqliteDataReader, Label>
	{
		public Label Convert(SqliteDataReader input)
		{
			throw new System.NotImplementedException();
		}
	}
}