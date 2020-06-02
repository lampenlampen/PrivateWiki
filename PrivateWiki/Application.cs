using PrivateWiki.Data;
using PrivateWiki.StorageBackend;
using SimpleInjector;
using Splat;

namespace PrivateWiki
{
	public class Application
	{
		public static readonly Application Instance = new Application();

		public AppSettings AppSettings { get; set; }

		public IGlobalNotificationManager GlobalNotificationManager { get; set; }

		public IGenericPageStorage StorageBackend { get; set; }

		public ILauncherImpl Launcher { get; set; }

		public IMutableDependencyResolver MDR { get; set; }

		public Container Container { get; }

		public Application()
		{
			MDR = Locator.CurrentMutable;

			Container = new Container();

			AppSettings = new AppSettings();
		}

		public void VerifyContainer()
		{
			Container.Verify();
		}

		public void Test()
		{
			var filesystemProvider = Container.GetInstance<IFilesystemProvider>();

			var file = new File("C:\\Users\\test.md");

			filesystemProvider.WriteTextAsync(file, "Test");
		}
	}
}