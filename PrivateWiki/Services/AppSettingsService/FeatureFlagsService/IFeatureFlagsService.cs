using PrivateWiki.ViewModels.Settings;

namespace PrivateWiki.Services.AppSettingsService.FeatureFlagsService
{
	public interface IFeatureFlagsService
	{
		// Feature Flag IsAssetsSyncEnabled
		/// <summary>
		/// Used to disable the <see cref="LFSBackupSyncTargetViewModel.IsAssetsSyncEnabled"/> UI until the operation is implemented.
		/// </summary>
		public bool IsAssetsSyncEnabled { get; }
	}
}