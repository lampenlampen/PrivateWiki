using System;
using System.Threading.Tasks;

namespace PrivateWiki
{
	public interface ILauncherImpl
	{
		Task<bool> LaunchFileAsync(string path);

		Task<bool> LaunchUriAsync(Uri uri);
	}
}