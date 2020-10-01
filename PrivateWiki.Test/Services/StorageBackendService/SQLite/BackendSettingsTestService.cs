using System;
using PrivateWiki.Services.AppSettingsService.BackendSettings;

namespace PrivateWiki.Test.Services.StorageBackendService.SQLite
{
	public class BackendSettingsTestService : IBackendSettingsService
	{
		private readonly string _path = $"{Guid.NewGuid().ToString()}.db";

		public string GetSqliteBackendPath() => _path;
	}
}