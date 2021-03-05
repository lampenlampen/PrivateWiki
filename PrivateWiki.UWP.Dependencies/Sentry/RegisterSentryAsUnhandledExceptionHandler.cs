using System;
using PrivateWiki.Core;
using PrivateWiki.Services.StartupTask;
using Sentry;

namespace PrivateWiki.UWP.Dependencies.Sentry
{
	public class RegisterSentryAsUnhandledExceptionHandler : IStartupTask
	{
		private readonly Windows.UI.Xaml.Application _application;

		public RegisterSentryAsUnhandledExceptionHandler(Windows.UI.Xaml.Application application)
		{
			_application = application;
		}

		public void Handle(Null _)
		{
			_application.UnhandledException += (_, args) =>
			{
				using (SentrySdk.PushScope())
				{
					SentrySdk.ConfigureScope(s => s.SetTag("UnhandledException", "true"));
					SentrySdk.CaptureException(args.Exception);
					if (!args.Handled) // Not handled yet
					{
						// App might crash so make sure we flush this event.
						SentrySdk.FlushAsync(TimeSpan.FromSeconds(2)).Wait();
					}
				}
			};
		}
	}
}