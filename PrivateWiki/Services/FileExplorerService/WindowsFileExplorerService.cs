using System.Threading.Tasks;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationLauncherService;

namespace PrivateWiki.Services.FileExplorerService
{
	public class WindowsFileExplorerService : IFileExplorerService
	{
		private readonly IApplicationLauncherService _launcher;

		public WindowsFileExplorerService(IApplicationLauncherService launcher)
		{
			_launcher = launcher;
		}

		public Task<bool> ShowFolderAsync(Folder folder)
		{
			return _launcher.LaunchFolder(folder);
		}
	}
}