using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using PrivateWiki.DataModels;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService;
using PrivateWiki.Services.KeyValueCaches;
using PrivateWiki.Services.SqliteStorage;
using Xunit;

namespace PrivateWiki.Test.Services.AppSettingsService.BackupSyncSettingsService
{
	public class BackupSyncSettingsServiceTest : IDisposable
	{
		private readonly string _dbPath;
		private readonly IPersistentKeyValueCache _cache;
		private readonly IBackupSyncTargetsJsonSerializer _serializer;

		public BackupSyncSettingsServiceTest()
		{
			_dbPath = "backupSyncSettingsServiceTest";
			_cache = new SqliteKeyValueCache(new SqliteDatabase(new SqliteStorageOptions {Path = $"{_dbPath}.db"}));
			_serializer = new BackupSyncTargetsJsonSerializer();
		}

		[Fact]
		public async void InsertTest()
		{
			var backup = new PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService.BackupSyncSettingsService(_cache, _serializer);

			var id1 = Guid.Parse("76f5f307-272e-4624-83e8-9d5e82dfc61d");
			var id2 = Guid.Parse("2ccf2385-d12b-4b89-b912-7ab3d2376b04");

			var a = new LfsBackupSyncTarget(id1, "Test 1", "Test 1.0", "C:\\1", SyncFrequency.Never, false, false);
			var b = new LfsBackupSyncTarget(id2, "Test 2", "Test 2.0", "C:\\2", SyncFrequency.Never, false, false);

			var expected = new List<IBackupSyncTarget> {a, b};

			await backup.AddOrUpdateTargetAsync(a);
			await backup.AddOrUpdateTargetAsync(b);

			var backup2 = new PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService.BackupSyncSettingsService(_cache, _serializer);

			await backup2.LoadTargets();

			var items = backup2.Targets2.Items;

			Assert.Contains(a, items);
			Assert.Contains(b, items);
		}

		[Fact]
		public void ValueEqualityLfsBackupSyncTarget()
		{
			var id = Guid.NewGuid();
			const string name = "asfsaf";
			const string description = "sdfgsdfgjhdsfg";
			var isEnabled = true;
			var isAssetsSyncEnabled = true;
			const string path = "C:\\users";
			var frequency = SyncFrequency.Never;

			var target1 = new LfsBackupSyncTarget(id, name, description, path, frequency, isEnabled, isAssetsSyncEnabled);
			var target2 = new LfsBackupSyncTarget(id, description, description, path, frequency, isEnabled, isAssetsSyncEnabled);
			var target3 = new BackupSyncTestTarget(id, description, description, path, frequency, isEnabled, isAssetsSyncEnabled);

			Assert.StrictEqual(target1, target2);
			Assert.NotStrictEqual(target1, target3);
			Assert.NotStrictEqual(target3, target2);
		}

		public void Dispose()
		{
			_cache.DeleteCache();
		}
	}
}