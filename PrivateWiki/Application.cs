using System;
using NodaTime;
using PrivateWiki.StorageBackend;
using SimpleInjector;

namespace PrivateWiki
{
	public class Application
	{
		public static readonly Application Instance = new Application();

		public AppSettings AppSettings { get; set; }

		public IGlobalNotificationManager GlobalNotificationManager { get; set; }

		[Obsolete]
		public IGenericPageStorage StorageBackend { get; set; }

		[Obsolete]
		public ILauncherImpl Launcher { get; set; }

		public Container Container { get; }

		public Application()
		{
			Container = new Container();
			
			Container.Register<IClock>(() => SystemClock.Instance, Lifestyle.Singleton);

			AppSettings = new AppSettings();
		}

		public void VerifyContainer()
		{
			Container.Verify();
		}
	}
}