using PrivateWiki.Services.Logger;

namespace PrivateWiki.Core.Logging
{
	public class LogCmdHandler : ICommandHandler<LogEntry>
	{
		private readonly ILogger _logger;

		public LogCmdHandler(ILogger logger)
		{
			_logger = logger;
		}

		public void Handle(LogEntry logEntry)
		{
			_logger.Log(logEntry);
		}
	}
}