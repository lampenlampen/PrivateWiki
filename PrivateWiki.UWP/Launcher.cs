using System;
using System.Threading.Tasks;
using Windows.Storage;
using PrivateWiki.Services.ApplicationLauncherService;

namespace PrivateWiki.UWP
{
	public class Launcher : IApplicationLauncherServiceImpl
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
	}
}