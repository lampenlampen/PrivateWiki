using System;
using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.ApplicationLauncherService
{
	public interface IApplicationLauncherService
	{
		Task<bool> LaunchFileAsync(string path);

		Task<bool> LaunchUriAsync(Uri uri);

		Task<bool> LaunchFolder(Folder folder);
	}
}