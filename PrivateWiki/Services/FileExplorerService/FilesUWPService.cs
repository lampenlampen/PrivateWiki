using System;
using System.Threading.Tasks;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationLauncherService;

namespace PrivateWiki.Services.FileExplorerService
{
	public class FilesUWPService : IFileExplorerService
	{
		private readonly IApplicationLauncherService _launcher;

		public FilesUWPService(IApplicationLauncherService launcher)
		{
			_launcher = launcher;
		}

		public Task<bool> ShowFolderAsync(Folder folder)
		{
			var uri = new Uri($"files-uwp:///{folder.Path}");

			return _launcher.LaunchUriAsync(uri);
		}
	}
}