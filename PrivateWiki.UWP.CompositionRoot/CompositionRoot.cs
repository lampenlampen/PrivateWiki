using PrivateWiki.DAL;
using PrivateWiki.Services.StartupTask;
using PrivateWiki.UWP.Dependencies.Sentry;
using SimpleInjector;

namespace PrivateWiki.UWP.CompositionRoot
{
	public static class CompositionRoot
	{
		public static void Bootstrap(Container container)
		{
			DALCompositionRoot.Bootstrap(container);
			PrivateWiki.CompositionRoot.Bootstrap(container);
			PrivateWiki.Dependencies.CompositionRoot.Bootstrap(container);

			container.Collection.Append<IStartupTask, RegisterSentryAsUnhandledExceptionHandler>();
		}
	}
}