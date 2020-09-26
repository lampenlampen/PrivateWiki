using System.Collections.Generic;

namespace PrivateWiki.Services.SqliteStorage.v2
{
	public class SqliteCommand
	{
		public string Sql { get; }

		public IList<KeyValuePair<string, string>> Parameters { get; }
	}
}