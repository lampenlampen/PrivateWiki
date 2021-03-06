using PrivateWiki.Core;
using PrivateWiki.Services.StartupTask;
using Sentry;

namespace PrivateWiki.Dependencies.Sentry
{
	public class RegisterSentryCmdHandler : IStartupTask
	{
		public void Handle(Null _)
		{
			var sentryOptions = new SentryOptions
			{
				Dsn = "https://538de75fb6dc4fd6819753186e6b3ecf@o528820.ingest.sentry.io/5646411",
				AttachStacktrace = true
			};

			SentrySdk.Init(sentryOptions);
		}
	}
}