using System;
using System.Threading.Tasks;
using PrivateWiki.Services.ApplicationLauncherService;

namespace PrivateWiki
{
	public class Launcher
	{
		private IApplicationLauncherServiceImpl _impl;

		public Launcher(IApplicationLauncherServiceImpl impl)
		{
		}

		public Task<bool> LaunchFileAsync(string path) => _impl.LaunchFileAsync(path);

		public Task<bool> LaunchUriAsync(Uri uri)
		{
			throw new NotImplementedException();
		}
	}
}