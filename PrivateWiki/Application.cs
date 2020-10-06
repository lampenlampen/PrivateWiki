using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using FluentResults;
using NodaTime;
using PrivateWiki.Core;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.AppSettingsService;
using PrivateWiki.Services.AppSettingsService.BackendSettings;
using PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService;
using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.AppSettingsService.MarkdownRenderingSettingsService;
using PrivateWiki.Services.Backends;
using PrivateWiki.Services.Backends.Sqlite;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.DefaultPagesService;
using PrivateWiki.Services.FileExplorerService;
using PrivateWiki.Services.GlobalNotificationService;
using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.LFSBackupService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageBackendService.SQLite;
using PrivateWiki.Services.StorageServices.Sql.Sqlite;
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
			Container.Register<IPageBackendService, PageBackendService>(Lifestyle.Singleton);
			Container.Register<IPageStorageBackendServiceImpl>(() => new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance), Lifestyle.Singleton);
			Container.Register<IDebugModeService, DebugModeService>(Lifestyle.Singleton);
			Container.Register<IApplicationLauncherService, ApplicationLauncherService>(Lifestyle.Singleton);
			Container.Register<IDefaultPagesService, DefaultPagesService>(Lifestyle.Transient);
			Container.Register<IAssetsService, AssetsService>(Lifestyle.Singleton);
			Container.Register<IMostRecentlyVisitedPagesService, MostRecentlyViewedPagesManager>(Lifestyle.Singleton);
			Container.Register<IInMemoryKeyValueCache, InMemoryCache>(Lifestyle.Singleton);
			Container.Register<IPersistentKeyValueCache>(() => new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions {Path = "settings.db"})),
				Lifestyle.Singleton);
			Container.Register<IFileExplorerService, FilesUWPService>();
			Container.Register<ILFSBackupService, LFSBackupService>(Lifestyle.Transient);
			Container.RegisterSingleton<IPageBackend>(() => new PageSqliteBackend(new SqliteDatabase(new SqliteStorageOptions {Path = "test.db"})));
			Container.RegisterSingleton<ILabelBackend, LabelSqliteBackend>();
			Container.RegisterSingleton<IPageLabelsBackend, PageLabelsSqliteBackend>();
			// TODO Lifestyle

			// Converter
			Container.RegisterSingleton<IConverter<DbDataReader, Label>, DbReaderToLabelConverter>();
			Container.RegisterSingleton<IConverter<DbDataReader, IList<Label>>, DbReaderToLabelsConverter>();
			Container.RegisterSingleton<IConverter<DbDataReader, IEnumerable<Guid>>, DbReaderToLabelIdsConverter>();


			// Settings
			Container.Register<IAppSettingsService, AppSettings>(Lifestyle.Singleton);
			Container.Register<IFeatureFlagsService, FeatureFlagsService>(Lifestyle.Singleton);
			Container.Register<IRenderingMarkdownSettingsService, RenderingMarkdownSettingsService>(Lifestyle.Singleton);
			Container.Register<IBackupSyncSettingsService, BackupSyncSettingsService>();
			Container.RegisterSingleton<IBackendSettingsService, BackendSettingsService>();


			// Serialization
			Container.Register<IBackupSyncTargetsJsonSerializer, BackupSyncTargetsJsonSerializer>(Lifestyle.Singleton);
		}

		public async Task Initialize()
		{
			Container.Verify();

			var defaultPageService = Container.GetInstance<IDefaultPagesService>();
			await defaultPageService.InsertDefaultPages();
		}
	}
}