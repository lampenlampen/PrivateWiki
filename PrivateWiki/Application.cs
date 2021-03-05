using System;
using PrivateWiki.Core;
using PrivateWiki.Services.AppSettingsService;
using PrivateWiki.Services.GlobalNotificationService;
using PrivateWiki.Services.StartupTask;
using ReactiveUI;
using SimpleInjector;
using Splat;

namespace PrivateWiki
{
	public class Application
	{
		public static readonly Application Instance = new Application();

		[Obsolete] public IAppSettingsService AppSettings { get; }

		public IGlobalNotificationManager GlobalNotificationManager { get; set; }

		public Container Container { get; set; }

		private Application()
		{
			Container = new Container();

			AppSettings = new AppSettings(Container);

			var compRoot = new CompositionRoot();
			compRoot.Init(Container);

			Locator.CurrentMutable.InitializeSplat();
			Locator.CurrentMutable.InitializeReactiveUI();
		}

		public void Initialize()
		{
			Container.Verify();

			var startupTasks = Container.GetInstance<IStartupTask>();
			startupTasks.Handle(Null.Instance);
		}
	}
}