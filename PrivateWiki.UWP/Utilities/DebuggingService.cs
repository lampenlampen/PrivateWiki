using System.Diagnostics;

namespace PrivateWiki.Utilities
{
	public abstract class IDebuggingService
	{
		public bool DebugMode => RunningInDebugMode();
		public abstract bool RunningInDebugMode();
	}

	public class DebuggingService : IDebuggingService
	{
		private bool _debugMode = false;

		public override bool RunningInDebugMode()
		{
			SetDebugModeVar();
			return _debugMode;
		}

		[Conditional("DEBUG")]
		private void SetDebugModeVar()
		{
			_debugMode = true;
		}
	}
}