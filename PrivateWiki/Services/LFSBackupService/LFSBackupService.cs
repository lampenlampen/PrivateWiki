using System.Threading.Tasks;
using NodaTime;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationDataService;
using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.FilesystemService;
using PrivateWiki.Services.StorageBackendService;

namespace PrivateWiki.Services.LFSBackupService
{
	public class LFSBackupService : ILFSBackupService
	{
		private readonly IFilesystemService _filesystem;
		private readonly IPageBackendService _pageBackend;
		private readonly IApplicationDataService _applicationData;
		private readonly IFeatureFlagsService _featureFlags;
		private readonly IClock _clock;

		public LFSBackupService(IFilesystemService filesystem, IPageBackendService pageBackend, IApplicationDataService applicationDataService, IFeatureFlagsService featureFlags, IClock clock)
		{
			_filesystem = filesystem;
			_pageBackend = pageBackend;
			_applicationData = applicationDataService;
			_featureFlags = featureFlags;
			_clock = clock;
		}

		public Task ExportAsync(LFSBackupServiceOptions options) => Task.Run(async () =>
		{
			var root = options.Folder;

			var pages = await _pageBackend.GetAllPagesAsync().ConfigureAwait(false);

			foreach (var page in pages)
			{
				// TODO Result
				var file = await _filesystem.CreateFileAsync(root, $"{page.Path.FullPath}{page.ContentType.FileExtension}").ConfigureAwait(false);

				await _filesystem.WriteTextAsync(file.Value, page.Content).ConfigureAwait(false);
			}

			// Feature Flag IsAssetsSyncEnabled
			if (_featureFlags.IsAssetsSyncEnabled && options.IsAssetsSyncEnabled)
			{
				var dataFolder = await _applicationData.GetDataFolderAsync().ConfigureAwait(false);
				var result = await _filesystem.GetAllFoldersAsync(dataFolder).ConfigureAwait(false);

				if (result.IsFailed) return;

				foreach (var folder in result.Value)
				{
					_filesystem.CopyAsync(folder, root);
				}
			}
		});

		public Task CreateBackupAsync(LFSBackupServiceOptions options) => Task.Run(async () =>
		{
			var instant = _clock.GetCurrentInstant();
			var tz = DateTimeZoneProviders.Tzdb.GetSystemDefault();
			var zonedDateTime = instant.InZone(tz);
			var folderName = $"{zonedDateTime.Year}.{zonedDateTime.Month}.{zonedDateTime.Day}-{zonedDateTime.Hour}.{zonedDateTime.Minute}";

			var rootFolderResult = await _filesystem.CreateFolderAsync(options.Folder, folderName).ConfigureAwait(false);

			if (rootFolderResult.IsFailed) return;

			await ExportAsync(new LFSBackupServiceOptions(options.IsAssetsSyncEnabled, new Folder(rootFolderResult.Value.Path, rootFolderResult.Value.Name)));
		});
	}
}