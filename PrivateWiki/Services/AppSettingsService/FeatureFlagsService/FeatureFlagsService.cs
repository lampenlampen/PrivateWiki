namespace PrivateWiki.Services.AppSettingsService.FeatureFlagsService
{
	public class FeatureFlagsService : IFeatureFlagsService
	{
		public bool IsAssetsSyncEnabled { get; } = true;
	}
}