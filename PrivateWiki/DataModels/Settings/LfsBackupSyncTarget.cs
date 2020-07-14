using System;

namespace PrivateWiki.DataModels.Settings
{
	public class LfsBackupSyncTarget
	{
		public Guid Id { get; }

		public string Name { get; }

		public string TargetPath { get; }

		public bool IsEnabled { get; }

		public bool IsAssetsSyncEnabled { get; }
	}
}