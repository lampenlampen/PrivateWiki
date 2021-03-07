using PrivateWiki.Core;
using PrivateWiki.Services.Logger;
using PrivateWiki.Services.StartupTask;

namespace PrivateWiki.UWP.Dependencies
{
	public class RegisterLoggerUncaughtExceptionHandler : IStartupTask
	{
		private readonly Windows.UI.Xaml.Application _application;

		private readonly ILogger _logger;

		public RegisterLoggerUncaughtExceptionHandler(Windows.UI.Xaml.Application application, ILogger logger)
		{
			_application = application;
			_logger = logger;
		}

		public void Handle(Null command)
		{
			_application.UnhandledException += (sender, args) => { _logger.Log(new LogEntry(LoggingEventType.Fatal, args.Message, args.Exception)); };
		}
	}
}