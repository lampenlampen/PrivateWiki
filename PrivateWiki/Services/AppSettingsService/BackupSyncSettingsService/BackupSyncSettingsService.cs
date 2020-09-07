using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Dawn;
using DynamicData;
using PrivateWiki.DataModels;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.SerializationService;
using ReactiveUI;

namespace PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService
{
	public class BackupSyncSettingsService : IBackupSyncSettingsService
	{
		private readonly IKeyValueCache _cache;

		private readonly IJsonSerializationService<IEnumerable<IBackupSyncTarget>> _serializer;

		private readonly ISourceCache<IBackupSyncTarget, Guid> _targets = new SourceCache<IBackupSyncTarget, Guid>(x => x.Id);

		public IObservableCache<IBackupSyncTarget, Guid> Targets2 => _targets.AsObservableCache();

		public BackupSyncSettingsService(IPersistentKeyValueCache cache, IBackupSyncTargetsJsonSerializer serializer)
		{
			_cache = cache;
			_serializer = serializer;

			LoadData();
		}

		private async Task LoadData()
		{
			var a = await _cache.GetObjectAsync("settings_backupSync_targets", _serializer);

			if (a.HasError<KeyNotFoundError>()) return;

			var data = a.Value;

			foreach (var target in data)
			{
				_targets.AddOrUpdate(target);
			}
		}

		public async Task RemoveTargetAsync(IBackupSyncTarget target)
		{
			Guard.Argument(target, nameof(target)).NotNull();

			_targets.Remove(target);

			var a = _targets.Items;

			if (!a.Any())
			{
				await _cache.RemoveAsync("settings_backupSync_targets");
			}
			else
			{
				await _cache.InsertAsync("settings_backupSync_targets", a, _serializer).ConfigureAwait(false);
			}
		}

		public async Task RemoveTargetAsync(Guid id)
		{
			_targets.Remove(id);

			var a = _targets.Items;

			if (!a.Any())
			{
				await _cache.RemoveAsync("settings_backupSync_targets");
			}
			else
			{
				await _cache.InsertAsync("settings_backupSync_targets", a, _serializer).ConfigureAwait(false);
			}
		}

		public async Task AddOrUpdateTargetAsync(IBackupSyncTarget target)
		{
			_targets.AddOrUpdate(target);

			var b = _targets.Items;
			await _cache.InsertAsync("settings_backupSync_targets", b, _serializer).ConfigureAwait(false);
		}

		public async Task LoadTargets()
		{
			await LoadData();
		}
	}
}