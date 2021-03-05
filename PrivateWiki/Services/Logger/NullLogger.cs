namespace PrivateWiki.Services.Logger
{
	public class NullLogger : ILogger
	{
		public void Log(LogEntry entry) { }
	}
}