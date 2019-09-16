using Models.Storage;
using StorageBackend.SQLite;

namespace PrivateWiki.Storage
{
	public static class DefaultStorageBackends
	{
		private static SqLiteStorage sqLiteStorage = new SqLiteStorage("main");
		
		private static SqLiteStorage testSqliteStorage = new SqLiteStorage("test");

		public static SqLiteStorage GetSqliteStorage()
		{
			return testSqliteStorage;
		}
	}
}