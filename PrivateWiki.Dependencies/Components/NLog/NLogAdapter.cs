using System;
using NLog;
using PrivateWiki.Services.Logger;
using ILogger = PrivateWiki.Services.Logger.ILogger;

namespace PrivateWiki.Dependencies.Components.NLog
{
	public class NLogAdapter : ILogger
	{
		private readonly Logger _logger;

		public NLogAdapter(Logger logger)
		{
			_logger = logger;
		}

		public void Log(LogEntry entry)
		{
			if (entry.Exception is null)
			{
				_logger.Log(entry.Severity.ToNLogSeverity(), entry.Message);
			}
			else
			{
				_logger.Log(entry.Severity.ToNLogSeverity(), entry.Exception, entry.Message);
			}
		}
	}

	internal static class LoggingEventTypeExtensions
	{
		internal static LogLevel ToNLogSeverity(this LoggingEventType severity)
		{
			return severity switch
			{
				LoggingEventType.Debug => LogLevel.Debug,
				LoggingEventType.Information => LogLevel.Info,
				LoggingEventType.Warning => LogLevel.Warn,
				LoggingEventType.Error => LogLevel.Error,
				LoggingEventType.Fatal => LogLevel.Fatal,
				_ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
			};
		}
	}
}