using System.Diagnostics;

namespace PrivateWiki.Services.DebugModeService
{
	public class DebugModeService : IDebugModeService
	{
		private bool _debugMode = false;

		public bool RunningInDebugMode()
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