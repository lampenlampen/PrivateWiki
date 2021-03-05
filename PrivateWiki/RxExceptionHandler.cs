using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using PrivateWiki.Core;
using PrivateWiki.Services.Logger;
using ReactiveUI;

namespace PrivateWiki
{
	public class RxExceptionHandler : IObserver<Exception>
	{
		private readonly ICommandHandler<LogEntry> _logger;

		public RxExceptionHandler(ICommandHandler<LogEntry> logger)
		{
			_logger = logger;
		}

		public void OnNext(Exception value)
		{
			if (Debugger.IsAttached) Debugger.Break();

			_logger.Handle(new LogEntry(LoggingEventType.Error, value.ToString(), value));

			RxApp.MainThreadScheduler.Schedule(() => throw value);
		}

		public void OnError(Exception error)
		{
			if (Debugger.IsAttached) Debugger.Break();

			_logger.Handle(new LogEntry(LoggingEventType.Error, error.ToString(), error));

			RxApp.MainThreadScheduler.Schedule(() => throw error);
		}

		public void OnCompleted()
		{
			if (Debugger.IsAttached) Debugger.Break();
			RxApp.MainThreadScheduler.Schedule(() => throw new NotImplementedException());
		}
	}
}