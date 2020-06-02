using System;
using System.Threading.Tasks;

namespace PrivateWiki
{
	public class Launcher
	{
		private ILauncherImpl _impl;

		public Launcher(ILauncherImpl impl)
		{
		}

		public Task<bool> LaunchFileAsync(string path) => _impl.LaunchFileAsync(path);

		public Task<bool> LaunchUriAsync(Uri uri)
		{
			throw new NotImplementedException();
		}
	}
}