using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using NLog;
using ReactiveUI;

namespace PrivateWiki
{
	public class RxExceptionHandler : IObserver<Exception>
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void OnNext(Exception value)
		{
			if (Debugger.IsAttached) Debugger.Break();

			Logger.Error(value);

			RxApp.MainThreadScheduler.Schedule(() => throw value);
		}

		public void OnError(Exception error)
		{
			if (Debugger.IsAttached) Debugger.Break();

			Logger.Error(error);

			RxApp.MainThreadScheduler.Schedule(() => throw error);
		}

		public void OnCompleted()
		{
			if (Debugger.IsAttached) Debugger.Break();
			RxApp.MainThreadScheduler.Schedule(() => throw new NotImplementedException());
		}
	}
}