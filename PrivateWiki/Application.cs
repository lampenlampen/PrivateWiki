using System.Threading.Tasks;
using FluentResults;
using NodaTime;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.AppSettingsService;
using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.DefaultPagesService;
using PrivateWiki.Services.FileExplorerService;
using PrivateWiki.Services.GlobalNotificationService;
using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.LFSBackupService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.SqliteStorage;
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

		private Application()
		{
			Container = new Container();

			AppSettings = new AppSettings(Container);

			var fluentResultLogger = new FluentResultLogger();
			Result.Setup(cfg => cfg.Logger = fluentResultLogger);

			Container.Register<IClock>(() => SystemClock.Instance, Lifestyle.Singleton);
			Container.Register<IPageBackendService, PageBackendService>(Lifestyle.Transient);
			Container.Register<IDebugModeService, DebugModeService>(Lifestyle.Singleton);
			Container.Register<IApplicationLauncherService, ApplicationLauncherService>(Lifestyle.Singleton);
			Container.Register<IDefaultPagesService, DefaultPagesService>(Lifestyle.Transient);
			Container.Register<IAssetsService, AssetsService>(Lifestyle.Singleton);
			Container.Register<IMostRecentlyVisitedPagesService, MostRecentlyViewedPagesManager>(Lifestyle.Singleton);
			Container.Register<IAppSettingsService, AppSettings>(Lifestyle.Singleton);
			Container.Register<IFeatureFlagsService, FeatureFlagsService>(Lifestyle.Singleton);
			Container.Register<IMarkdownRenderingSettingsService, MarkdownRenderingSettings>(Lifestyle.Singleton);
			Container.Register<IInMemoryKeyValueCache, InMemoryCache>(Lifestyle.Singleton);
			Container.Register<IPersistentKeyValueCache>(() => new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions {Path = "settings.db"})), Lifestyle.Singleton);
			Container.Register<IFileExplorerService, FilesUWPService>();
			Container.Register<ILFSBackupService, LFSBackupService>(Lifestyle.Transient);
		}

		public async Task Initialize()
		{
			Container.Verify();

			var defaultPageService = Container.GetInstance<IDefaultPagesService>();
			await defaultPageService.InsertDefaultPages();
		}
	}
}