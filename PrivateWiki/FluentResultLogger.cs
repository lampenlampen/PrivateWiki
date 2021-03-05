using FluentResults;
using PrivateWiki.Core;
using PrivateWiki.Services.Logger;

namespace PrivateWiki
{
	public class FluentResultLogger : IResultLogger
	{
		private readonly ICommandHandler<LogEntry> _logger;

		public FluentResultLogger(ICommandHandler<LogEntry> logger)
		{
			_logger = logger;
		}

		public void Log(string context, ResultBase result)
		{
			_logger.Handle(new LogEntry(LoggingEventType.Information, result.ToString()));
		}
	}
}