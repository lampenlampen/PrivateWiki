using PrivateWiki.Core;
using PrivateWiki.Services.StartupTask;
using SimpleInjector;

namespace PrivateWiki
{
	public static class ServiceLocator
	{
		public static Container Container { get; set; }

		public static void Initialize()
		{
			Container.Verify();

			var startupTasks = Container.GetInstance<IStartupTask>();
			startupTasks.Handle(Null.Instance);
		}
	}
}