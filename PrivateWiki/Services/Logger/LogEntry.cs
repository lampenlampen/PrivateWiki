using System;
using Microsoft.Toolkit.Diagnostics;

namespace PrivateWiki.Services.Logger
{
	public class LogEntry
	{
		public LoggingEventType Severity { get; }
		public string Message { get; }
		public Exception? Exception { get; }

		public LogEntry(LoggingEventType severity, string message, Exception? exception = null)
		{
			Guard.IsNotNullOrEmpty(message, nameof(message));

			Severity = severity;
			Message = message;
			Exception = exception;
		}
	}
}