using System.Diagnostics;

namespace PrivateWiki.Core.DebugMode
{
	public class DebugModeQueryHandler : IQueryHandler<GetDebugMode, DebugMode>
	{
		private bool _debugMode = false;

		public DebugMode Handle(GetDebugMode query)
		{
			SetDebugModeVar();
			return new DebugMode(_debugMode);
		}

		[Conditional("DEBUG")]
		private void SetDebugModeVar()
		{
			_debugMode = true;
		}
	}
}