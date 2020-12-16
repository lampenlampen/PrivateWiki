using System;
using System.Collections.Generic;
using System.Data.Common;
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
using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.LFSBackupService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.PackageService;
using PrivateWiki.Services.StartupTask;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageBackendService.SQLite;
using PrivateWiki.Services.StorageServices.Sql.Sqlite;
using PrivateWiki.Services.TranslationService;
using PrivateWiki.ViewModels.Controls;
using SimpleInjector;

namespace PrivateWiki
{
	public class CompositionRoot
	{
		public void Init(Container container)
		{
			container.Register<IClock>(() => SystemClock.Instance, Lifestyle.Singleton);
			container.Register<IPageBackendService, PageBackendService>(Lifestyle.Singleton);
			container.Register<IPageStorageBackendServiceImpl>(() => new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance), Lifestyle.Singleton);
			container.Register<IDebugModeService, DebugModeService>(Lifestyle.Singleton);
			container.Register<IApplicationLauncherService, ApplicationLauncherService>(Lifestyle.Singleton);
			container.Register<IDefaultPagesService, DefaultPagesService>(Lifestyle.Transient);
			container.Register<IAssetsService, AssetsService>(Lifestyle.Singleton);
			container.Register<IMostRecentlyVisitedPagesService, MostRecentlyViewedPagesManager>(Lifestyle.Singleton);
			container.Register<IInMemoryKeyValueCache, InMemoryCache>(Lifestyle.Singleton);
			container.Register<IPersistentKeyValueCache>(() => new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions {Path = "settings.db"})),
				Lifestyle.Singleton);
			container.Register<IFileExplorerService, FilesUWPService>();
			container.Register<ILFSBackupService, LFSBackupService>(Lifestyle.Transient);
			container.RegisterSingleton<IPageBackend>(() => new PageSqliteBackend(new SqliteDatabase(new SqliteStorageOptions {Path = "test.db"})));
			container.RegisterSingleton<ILabelBackend, LabelSqliteBackend>();
			// container.RegisterSingleton<ILabelBackend, TestBackend>();
			container.RegisterSingleton<IPageLabelsBackend, PageLabelsSqliteBackend>();
			// container.RegisterSingleton<IPageLabelsBackend, TestBackend>();
			// TODO Lifestyle

			// Converter
			container.RegisterSingleton<IConverter<DbDataReader, Label>, DbReaderToLabelConverter>();
			container.RegisterSingleton<IConverter<DbDataReader, IList<Label>>, DbReaderToLabelsConverter>();
			container.RegisterSingleton<IConverter<DbDataReader, IEnumerable<Guid>>, DbReaderToLabelIdsConverter>();

			// Settings
			container.Register<IAppSettingsService, AppSettings>(Lifestyle.Singleton);
			container.Register<IFeatureFlagsService, FeatureFlagsService>(Lifestyle.Singleton);
			container.Register<IRenderingMarkdownSettingsService, RenderingMarkdownSettingsService>(Lifestyle.Singleton);
			container.Register<IBackupSyncSettingsService, BackupSyncSettingsService>();
			container.RegisterSingleton<IBackendSettingsService, BackendSettingsService>();

			// Serialization
			container.Register<IBackupSyncTargetsJsonSerializer, BackupSyncTargetsJsonSerializer>(Lifestyle.Singleton);

			// Startup Tasks
			container.RegisterSingleton<IStartupTask, CompositeStartupTask>();
			container.Collection.Register<IStartupTask>(new[] {typeof(InsertDefaultPagesStartupTask), typeof(FluentResultLoggerStartupTask)}, Lifestyle.Singleton);

			container.RegisterSingleton<TranslationResources, InCodeTranslationResources>();

			// Bug see https://github.com/reactiveui/splat/issues/597
			/*
			Container.UseSimpleInjectorDependencyResolver();
			Locator.CurrentMutable.InitializeSplat();
			Locator.CurrentMutable.InitializeReactiveUI();
			*/


			// ViewModels
			container.Register<AddLabelsToPageControlViewModel>();
		}
	}
}