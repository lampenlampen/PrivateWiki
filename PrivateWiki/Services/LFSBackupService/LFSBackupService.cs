using System.Threading.Tasks;
using NLog;
using PrivateWiki.DataModels;
using PrivateWiki.Services.FilesystemService;
using PrivateWiki.Services.StorageBackendService;

namespace PrivateWiki.Services.LFSBackupService
{
	public class LFSBackupService : ILFSBackupService
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private IFilesystemService _filesystem;
		private IPageBackendService _pageBackend;

		public LFSBackupService(IFilesystemService filesystem, IPageBackendService pageBackend)
		{
			_filesystem = filesystem;
			_pageBackend = pageBackend;
		}

		public Task Sync(LFSBackupServiceOptions options) => Task.Run(async () =>
		{
			var root = options.Folder;

			var pages = await _pageBackend.GetAllPagesAsync().ConfigureAwait(false);

			foreach (var page in pages)
			{
				// TODO Result
				var file = await _filesystem.CreateFile(root, $"{page.Path.FullPath}{page.ContentType.FileExtension}").ConfigureAwait(false);

				await _filesystem.WriteTextAsync(file.Value, page.Content).ConfigureAwait(false);
			}

			// TODO Sync Assets
		});
	}

	public class LFSBackupServiceOptions
	{
		public LFSBackupServiceOptions(bool isAssetsSyncEnabled, Folder folder)
		{
			IsAssetsSyncEnabled = isAssetsSyncEnabled;
			Folder = folder;
		}

		public bool IsAssetsSyncEnabled { get; }

		public Folder Folder { get; }
	}
}