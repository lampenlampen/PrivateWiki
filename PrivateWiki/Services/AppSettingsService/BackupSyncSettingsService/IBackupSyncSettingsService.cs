using System;
using System.Threading.Tasks;
using DynamicData;
using PrivateWiki.DataModels.Settings;

namespace PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService
{
	public interface IBackupSyncSettingsService
	{
		IObservableCache<IBackupSyncTarget, Guid> Targets2 { get; }

		public Task RemoveTargetAsync(IBackupSyncTarget target);
		public Task RemoveTargetAsync(Guid id);

		public Task AddOrUpdateTargetAsync(IBackupSyncTarget target);

		public Task LoadTargets();
	}
}