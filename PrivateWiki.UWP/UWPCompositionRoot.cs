using PrivateWiki.Services.ApplicationDataService;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.FilesystemService;
using PrivateWiki.UWP.Services.FilesystemService;
using SimpleInjector;
using ApplicationLauncherService = PrivateWiki.UWP.Services.ApplicationLauncherService.ApplicationLauncherService;

namespace PrivateWiki.UWP
{
	public class UWPCompositionRoot
	{
		public void Init(Container container)
		{
			container.Register<IFilesystemService, UWPFullTrustFilesystemService>(Lifestyle.Singleton);
			container.Register<IApplicationLauncherServiceImpl, ApplicationLauncherService>(Lifestyle.Singleton);
			container.Register<IApplicationDataService, ApplicationDataService>(Lifestyle.Singleton);
		}
	}
}