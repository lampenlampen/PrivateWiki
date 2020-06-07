using System;
using System.Threading.Tasks;

namespace PrivateWiki.Services.ApplicationLauncherService
{
	public interface IApplicationLauncherServiceImpl
	{
		Task<bool> LaunchFileAsync(string path);

		Task<bool> LaunchUriAsync(Uri uri);
	}
}