using SimpleInjector;

namespace PrivateWiki.Services.SqliteStorage
{
	public class SqliteDatabaseFactory
	{
		private readonly Container _container;

		public SqliteDatabaseFactory(Container container)
		{
			_container = container;
		}

		public ISqliteStorage Build(string path)
		{
			var options = new SqliteStorageOptions {Path = path};
			
			return new SqliteDatabase(options);
		}
	}
}