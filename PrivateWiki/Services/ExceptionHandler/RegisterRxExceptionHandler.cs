using PrivateWiki.Core;
using PrivateWiki.Services.Logger;
using PrivateWiki.Services.StartupTask;
using ReactiveUI;

namespace PrivateWiki.Services.ExceptionHandler
{
	public class RegisterRxExceptionHandler : IStartupTask
	{
		private readonly ICommandHandler<LogEntry> _logger;

		public RegisterRxExceptionHandler(ICommandHandler<LogEntry> logger)
		{
			_logger = logger;
		}

		public void Handle(Null _)
		{
			RxApp.DefaultExceptionHandler = new RxExceptionHandler(_logger);
		}
	}
}