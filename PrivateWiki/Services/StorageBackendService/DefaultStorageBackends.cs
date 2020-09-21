using PrivateWiki.Services.StorageBackendService.SQLite;

namespace PrivateWiki.Services.StorageBackendService
{
	public static class DefaultStorageBackends
	{
		private static SqLiteStorageOptions sqLiteStorage = new SqLiteStorageOptions("main");

		private static SqLiteStorageOptions testSqliteStorage = new SqLiteStorageOptions("test");

		public static SqLiteStorageOptions GetSqliteStorage()
		{
			return testSqliteStorage;
		}
	}
}