using Microsoft.Toolkit.Diagnostics;
using PrivateWiki.Services.ApplicationDataService;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.FilesystemService;
using PrivateWiki.Services.GlobalNotificationService;
using PrivateWiki.UWP.UI.Services.FilesystemService;
using PrivateWiki.UWP.UI.UI;
using RavinduL.LocalNotifications;
using SimpleInjector;

namespace PrivateWiki.UWP.UI
{
	public static class UwpCompositionRoot
	{
		public static void Bootstrap(Container container)
		{
			Guard.IsNotNull(container, nameof(container));

			container.Register(() => Windows.UI.Xaml.Application.Current, Lifestyle.Singleton);
			container.Register<IFilesystemService, UWPFullTrustFilesystemService>(Lifestyle.Singleton);
			container.Register<IApplicationLauncherServiceImpl, Services.ApplicationLauncherService.ApplicationLauncherService>(Lifestyle.Singleton);
			container.Register<IApplicationDataService, ApplicationDataService>(Lifestyle.Singleton);
			container.RegisterSingleton<IGlobalNotificationManager, GlobalNotificationManager>();
			container.RegisterSingleton<LocalNotificationManager>(() => new LocalNotificationManager(App.Current.NotificationGrid));
		}
	}
}