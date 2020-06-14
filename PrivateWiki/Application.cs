using System.Threading.Tasks;
using NodaTime;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.AppSettingsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.DefaultPagesService;
using PrivateWiki.Services.FileExplorerService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.StorageBackendService;
using SimpleInjector;

namespace PrivateWiki
{
	public class Application
	{
		public static readonly Application Instance = new Application();

		public IAppSettingsService AppSettings { get; }

		public IGlobalNotificationManager GlobalNotificationManager { get; set; }

		public Container Container { get; }

		public Application()
		{
			Container = new Container();

			AppSettings = new Services.AppSettingsService.AppSettings(Container);

			Container.Register<IClock>(() => SystemClock.Instance, Lifestyle.Singleton);
			Container.Register<IPageBackendService, PageBackendService>(Lifestyle.Transient);
			Container.Register<IDebugModeService, DebugModeService>(Lifestyle.Singleton);
			Container.Register<IApplicationLauncherService, ApplicationLauncherService>(Lifestyle.Singleton);
			Container.Register<IDefaultPagesService, DefaultPagesService>(Lifestyle.Transient);
			Container.Register<IAssetsService, AssetsService>(Lifestyle.Singleton);
			Container.Register<IMostRecentlyVisitedPagesService, MostRecentlyViewedPagesManager>(Lifestyle.Singleton);
			Container.Register<IAppSettingsService, Services.AppSettingsService.AppSettings>(Lifestyle.Singleton);
			Container.Register<IMarkdownRenderingSettingsService, Services.AppSettingsService.MarkdownRenderingSettingsService.MarkdownRenderingSettings>(Lifestyle.Singleton);
			Container.Register<IFileExplorerService, FilesUWPService>();
		}

		public async Task Initialize()
		{
			Container.Verify();

			var defaultPageService = Container.GetInstance<IDefaultPagesService>();
			await defaultPageService.InsertDefaultPages();
		}
	}
}