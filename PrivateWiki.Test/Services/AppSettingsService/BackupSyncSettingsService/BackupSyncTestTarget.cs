using System;
using PrivateWiki.DataModels;
using PrivateWiki.DataModels.Settings;

namespace PrivateWiki.Test.Services.AppSettingsService.BackupSyncSettingsService
{
	internal class BackupSyncTestTarget : LfsBackupSyncTarget
	{
		public BackupSyncTestTarget(Guid id, string name, string description, string targetPath, SyncFrequency frequency, bool isEnabled, bool isAssetsSyncEnabled) : base(id, name, description, targetPath, frequency, isEnabled, isAssetsSyncEnabled)
		{
		}

		public BackupSyncTestTarget(string name, string description, string targetPath, SyncFrequency frequency, bool isEnabled, bool isAssetsSyncEnabled) : base(name, description, targetPath, frequency, isEnabled, isAssetsSyncEnabled)
		{
		}
	}
}