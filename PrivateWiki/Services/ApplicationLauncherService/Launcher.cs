using System;
using System.Threading.Tasks;

namespace PrivateWiki.Services.ApplicationLauncherService
{
	public class ApplicationLauncherService : IApplicationLauncherService
	{
		private IApplicationLauncherServiceImpl _impl;

		public ApplicationLauncherService(IApplicationLauncherServiceImpl impl)
		{
		}

		public Task<bool> LaunchFileAsync(string path) => _impl.LaunchFileAsync(path);

		public Task<bool> LaunchUriAsync(Uri uri)
		{
			throw new NotImplementedException();
		}
	}
}