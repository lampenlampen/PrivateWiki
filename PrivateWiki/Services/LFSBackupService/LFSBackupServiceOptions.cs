using PrivateWiki.DataModels;

namespace PrivateWiki.Services.LFSBackupService
{
	public class LFSBackupServiceOptions
	{
		public LFSBackupServiceOptions(bool isAssetsSyncEnabled, Folder folder)
		{
			IsAssetsSyncEnabled = isAssetsSyncEnabled;
			Folder = folder;
		}

		public bool IsAssetsSyncEnabled { get; }

		public Folder Folder { get; }
	}
}