namespace PrivateWiki.Services.DebugModeService
{
	// TODO convert IDebugModeService to IQueryHandler<GetDebugMode>
	public interface IDebugModeService
	{
		public bool RunningInDebugMode();
	}
}