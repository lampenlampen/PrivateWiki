namespace PrivateWiki.UWP.StorageBackend
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