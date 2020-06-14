using System.Threading.Tasks;
using NodaTime;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.DefaultPagesService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.StorageBackendService;
using SimpleInjector; //using PrivateWiki.Services.FileExplorerService;

namespace PrivateWiki
{
	public class Application
	{
		public static readonly Application Instance = new Application();

		public AppSettings AppSettings { get; set; }

		public IGlobalNotificationManager GlobalNotificationManager { get; set; }

		public Container Container { get; }

		public Application()
		{
			AppSettings = new AppSettings();

			Container = new Container();

			Container.Register<IClock>(() => SystemClock.Instance, Lifestyle.Singleton);
			Container.Register<IPageBackendService, PageBackendService>(Lifestyle.Transient);
			Container.Register<IDebugModeService, DebugModeService>(Lifestyle.Singleton);
			Container.Register<IApplicationLauncherService, ApplicationLauncherService>(Lifestyle.Singleton);
			Container.Register<IDefaultPagesService, DefaultPagesService>(Lifestyle.Transient);
			Container.Register<IAssetsService, AssetsService>(Lifestyle.Singleton);
			Container.Register<IMostRecentlyVisitedPagesService, MostRecentlyViewedPagesManager>(Lifestyle.Singleton);
		}

		public async Task Initialize()
		{
			Container.Verify();

			var defaultPageService = Container.GetInstance<IDefaultPagesService>();
			await defaultPageService.InsertDefaultPages();
		}
	}
}