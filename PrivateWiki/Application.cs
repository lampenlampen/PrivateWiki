using NodaTime;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.StorageBackendService;
using SimpleInjector;

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
			Container = new Container();

			Container.Register<IClock>(() => SystemClock.Instance, Lifestyle.Singleton);
			Container.Register<IPageBackendService, PageBackendService>(Lifestyle.Transient);
			Container.Register<IDebugModeService, DebugModeService>(Lifestyle.Singleton);

			AppSettings = new AppSettings();
		}

		public void VerifyContainer()
		{
			Container.Verify();
		}
	}
}