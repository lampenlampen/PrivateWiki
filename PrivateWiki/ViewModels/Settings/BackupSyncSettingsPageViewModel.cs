using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class BackupSyncSettingsPageViewModel : ReactiveObject
	{
		private IBackupSyncTargetViewModel? _backupSyncTargetViewModel;

		public readonly ObservableCollection<IBackupSyncTargetViewModel> Targets = new ObservableCollection<IBackupSyncTargetViewModel>();

		public readonly ReactiveCommand<BackupSyncTargetType, Unit> AddTarget;

		public readonly ReactiveCommand<Guid, Unit> RemoveTarget;

		public readonly ReactiveCommand<IBackupSyncTargetViewModel, Unit> SelectionChanged;

		public readonly ReactiveCommand<Unit, Unit> SaveConfigurations;

		private readonly ISubject<IBackupSyncTargetViewModel> _onDisplayBackupSyncTarget;
		public IObservable<IBackupSyncTargetViewModel> OnDisplayBackupSyncTarget => _onDisplayBackupSyncTarget;

		public BackupSyncSettingsPageViewModel()
		{
			AddTarget = ReactiveCommand.CreateFromTask<BackupSyncTargetType>(AddTargetAsync);
			RemoveTarget = ReactiveCommand.CreateFromTask<Guid>(RemoveTargetAsync);
			SelectionChanged = ReactiveCommand.CreateFromTask<IBackupSyncTargetViewModel>(SelectionChangedAsync);
			SaveConfigurations = ReactiveCommand.CreateFromTask(SaveTargetConfigurationsAsync);

			_onDisplayBackupSyncTarget = new Subject<IBackupSyncTargetViewModel>();
		}

		private async Task AddTargetAsync(BackupSyncTargetType type)
		{
			Targets.Add(new LFSBackupSyncTargetViewModel
			{
				Name = "Test1",
				Description = "Sync to folder test",
				TargetPath = "C:\\test",
				IsAssetsSyncEnabled = false,
				IsEnabled = false
			});
		}

		private async Task RemoveTargetAsync(Guid id)
		{
		}

		private async Task SelectionChangedAsync(IBackupSyncTargetViewModel target)
		{
			switch (target.Type)
			{
				case BackupSyncTargetType.LocalFileStorage:
					_backupSyncTargetViewModel = target;
					_onDisplayBackupSyncTarget.OnNext(_backupSyncTargetViewModel);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private async Task SaveTargetConfigurationsAsync()
		{
			foreach (var target in Targets)
			{
				target.SaveConfiguration.Execute().Subscribe();
			}
		}
	}

	public enum BackupSyncTargetType
	{
		LocalFileStorage
	}
}