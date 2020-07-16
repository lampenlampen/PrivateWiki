using FluentResults;
using NLog;

namespace PrivateWiki
{
	public class FluentResultLogger : IResultLogger
	{
		public static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public void Log(string context, ResultBase result)
		{
			Logger.Info(result);
		}
	}
}