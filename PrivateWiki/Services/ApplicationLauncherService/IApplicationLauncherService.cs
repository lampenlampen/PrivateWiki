using System;
using System.Threading.Tasks;

namespace PrivateWiki.Services.ApplicationLauncherService
{
	public interface IApplicationLauncherService
	{
		public Task<bool> LaunchFileAsync(string path);

		public Task<bool> LaunchUriAsync(Uri uri);
	}
}