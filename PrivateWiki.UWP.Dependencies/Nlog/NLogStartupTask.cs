using PrivateWiki.Core;
using PrivateWiki.Services.StartupTask;

namespace PrivateWiki.UWP.Dependencies.Nlog
{
	public class NLogStartupTask : IStartupTask
	{
		public void Handle(Null command)
		{
			Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
			NLog.LogManager.Configuration.Variables["LogPath"] = storageFolder.Path;
		}
	}
}