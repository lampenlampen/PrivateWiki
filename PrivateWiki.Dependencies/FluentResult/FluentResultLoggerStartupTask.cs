using FluentResults;
using PrivateWiki.Core;
using PrivateWiki.Services.Logger;
using PrivateWiki.Services.StartupTask;

namespace PrivateWiki.Dependencies.FluentResult
{
	public class FluentResultLoggerStartupTask : IStartupTask
	{
		private readonly ICommandHandler<LogEntry> _logger;

		public FluentResultLoggerStartupTask(ICommandHandler<LogEntry> logger)
		{
			_logger = logger;
		}

		public void Handle(Null _)
		{
			var fluentResultLogger = new FluentResultLogger(_logger);
			Result.Setup(cfg => cfg.Logger = fluentResultLogger);
		}
	}
}