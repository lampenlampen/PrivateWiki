using Models.Storage;

namespace PrivateWiki.Storage
{
	public static class DefaultStorageBackends
	{
		private static SqLiteStorage sqLiteStorage = new SqLiteStorage("main");
		
		private static SqLiteStorage testSqliteStorage = new SqLiteStorage("test");
		
		public static GitStorage testGitStorage = new GitStorage("https://gitlab.com/lampenlampen/gitbackendtest.git", "./storage/git_repo", "Felix Test", "lampen.lampen@outlook.de");

		public static SqLiteStorage GetSqliteStorage()
		{
			return testSqliteStorage;
		}
	}
}