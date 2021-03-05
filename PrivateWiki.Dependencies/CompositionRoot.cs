using PrivateWiki.Dependencies.Components.NLog;
using PrivateWiki.Dependencies.FluentResult;
using PrivateWiki.Dependencies.Sentry;
using PrivateWiki.Services.Logger;
using PrivateWiki.Services.StartupTask;
using SimpleInjector;

namespace PrivateWiki.Dependencies
{
	public static class CompositionRoot
	{
		public static void Bootstrap(Container container)
		{
			container.RegisterSingleton<ILogger, NLogAdapter>();
			container.Collection.Append<IStartupTask, FluentResultLoggerStartupTask>(Lifestyle.Singleton);
			container.Collection.Append<IStartupTask, RegisterSentryCmdHandler>();
		}
	}
}