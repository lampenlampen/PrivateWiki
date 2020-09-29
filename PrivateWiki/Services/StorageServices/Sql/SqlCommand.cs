using System.Collections.Generic;

namespace PrivateWiki.Services.StorageServices.Sql
{
	public class SqlCommand
	{
		public string Sql { get; }

		public IList<KeyValuePair<string, string>> Parameters { get; }

		public SqlCommand(string sql, IList<KeyValuePair<string, string>> parameters)
		{
			Sql = sql;
			Parameters = parameters;
		}
	}
}