using System;
using PrivateWiki.ViewModels.Settings;

namespace PrivateWiki.DataModels.Settings
{
	public class LfsBackupSyncTarget : IBackupSyncTarget
	{
		public Guid Id { get; }

		public string Name { get; }

		public string Description { get; }

		public string TargetPath { get; }

		public SyncFrequency Frequency { get; }

		public bool IsEnabled { get; }

		public bool IsAssetsSyncEnabled { get; }

		public BackupSyncTargetType Type { get; } = BackupSyncTargetType.LocalFileStorage;

		public LfsBackupSyncTarget(Guid id, string name, string description, string targetPath, SyncFrequency frequency, bool isEnabled, bool isAssetsSyncEnabled)
		{
			Id = id;
			Name = name;
			Description = description;
			TargetPath = targetPath;
			Frequency = frequency;
			IsEnabled = isEnabled;
			IsAssetsSyncEnabled = isAssetsSyncEnabled;
		}

		public LfsBackupSyncTarget(string name, string description, string targetPath, SyncFrequency frequency, bool isEnabled, bool isAssetsSyncEnabled)
		{
			Id = Guid.NewGuid();
			Name = name;
			Description = description;
			TargetPath = targetPath;
			Frequency = frequency;
			IsEnabled = isEnabled;
			IsAssetsSyncEnabled = isAssetsSyncEnabled;
		}

		protected bool Equals(LfsBackupSyncTarget other) => Id.Equals(other.Id);

		public override bool Equals(object? obj) => obj is LfsBackupSyncTarget other && other.GetType() == GetType() && Equals(other);

		public override int GetHashCode()
		{
			return Id.GetHashCode();
		}
	}

	public interface IBackupSyncTarget
	{
		public BackupSyncTargetType Type { get; }

		public Guid Id { get; }

		public string Name { get; }

		public string Description { get; }

		public bool IsEnabled { get; }

		public bool Equals(object other);

		public int GetHashCode();
	}
}