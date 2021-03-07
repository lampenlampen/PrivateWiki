using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.System;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationLauncherService;

namespace PrivateWiki.UWP.UI.Services.ApplicationLauncherService
{
	public class ApplicationLauncherService : IApplicationLauncherServiceImpl
	{
		public async Task<bool> LaunchFileAsync(string path)
		{
			var file = StorageFile.GetFileFromPathAsync(path);

			return await Windows.System.Launcher.LaunchFileAsync(await file);
		}

		public Task<bool> LaunchUriAsync(Uri uri)
		{
			return Windows.System.Launcher.LaunchUriAsync(uri).AsTask();
		}

		public async Task<bool> LaunchFolderAsync(Folder folder)
		{
			var folder2 = await StorageFolder.GetFolderFromPathAsync(folder.Path);

			return await Launcher.LaunchFolderAsync(folder2).AsTask().ConfigureAwait(false);
		}
	}
}