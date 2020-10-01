namespace PrivateWiki.Services.AppSettingsService.BackendSettings
{
	public class BackendSettingsService : IBackendSettingsService
	{
		public string GetSqliteBackendPath()
		{
			return "test.db";
		}
	}
}