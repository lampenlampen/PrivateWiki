using System;
using System.Collections.Generic;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using PrivateWiki.DataModels.Settings;
using ReactiveUI;

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