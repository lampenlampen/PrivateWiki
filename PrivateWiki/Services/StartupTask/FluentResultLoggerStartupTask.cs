using System.Threading.Tasks;
using FluentResults;

namespace PrivateWiki.Services.StartupTask
{
	public class FluentResultLoggerStartupTask : IStartupTask
	{
		public Task<bool> Execute()
		{
			var fluentResultLogger = new FluentResultLogger();
			Result.Setup(cfg => cfg.Logger = fluentResultLogger);

			return Task.FromResult(true);
		}
	}
}