using System;
using System.Threading.Tasks;
using PrivateWiki.DataModels;

namespace PrivateWiki.Services.ApplicationLauncherService
{
	public interface IApplicationLauncherServiceImpl
	{
		Task<bool> LaunchFileAsync(string path);

		Task<bool> LaunchUriAsync(Uri uri);

		Task<bool> LaunchFolderAsync(Folder folder);
	}
}