using NLog;
using PrivateWiki.Dependencies.Components.NLog;
using SimpleInjector;
using ILogger = PrivateWiki.Services.Logger.ILogger;

namespace PrivateWiki.Dependencies
{
	public class CompositionRoot
	{
		public static void Bootstrap(Container container)
		{
			container.RegisterSingleton<ILogger, NLogAdapter>();
			container.RegisterSingleton<Logger>();
		}
	}
}