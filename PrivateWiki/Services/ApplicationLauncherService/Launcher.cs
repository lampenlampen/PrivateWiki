using System;
using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.ApplicationLauncherService
{
	public class ApplicationLauncherService : IApplicationLauncherService
	{
		private IApplicationLauncherServiceImpl _impl;

		public ApplicationLauncherService(IApplicationLauncherServiceImpl impl)
		{
			_impl = impl;
		}

		public Task<bool> LaunchFileAsync(string path) => _impl.LaunchFileAsync(path);

		public Task<bool> LaunchUriAsync(Uri uri)
		{
			return _impl.LaunchUriAsync(uri);
		}

		public Task<bool> LaunchFolder(Folder folder)
		{
			return _impl.LaunchFolderAsync(folder);
		}
	}
}