using Microsoft.Toolkit.Diagnostics;
using PrivateWiki.Services.ApplicationDataService;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.FilesystemService;
using PrivateWiki.UWP.Services.FilesystemService;
using SimpleInjector;
using ApplicationLauncherService = PrivateWiki.UWP.Services.ApplicationLauncherService.ApplicationLauncherService;

namespace PrivateWiki.UWP
{
	public static class UwpCompositionRoot
	{
		public static void Bootstrap(Container container)
		{
			Guard.IsNotNull(container, nameof(container));

			container.Register<IFilesystemService, UWPFullTrustFilesystemService>(Lifestyle.Singleton);
			container.Register<IApplicationLauncherServiceImpl, ApplicationLauncherService>(Lifestyle.Singleton);
			container.Register<IApplicationDataService, ApplicationDataService>(Lifestyle.Singleton);
		}
	}
}