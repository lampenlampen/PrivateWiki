using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.AppSettingsService.BackupSyncSettingsService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class BackupSyncSettingsPageViewModel : ReactiveObject
	{
		private readonly IBackupSyncSettingsService _backupSyncSettings;

		private IBackupSyncTargetViewModel? _backupSyncTargetViewModel;

		private readonly ISourceList<IBackupSyncTargetViewModel> _source = new SourceList<IBackupSyncTargetViewModel>();

		public ObservableCollection<IBackupSyncTargetViewModel> Targets { get; } = new ObservableCollection<IBackupSyncTargetViewModel>();

		private IBackupSyncTargetViewModel? _selectedBackupSyncTarget;

		public IBackupSyncTargetViewModel? SelectedBackupSyncTarget
		{
			get => _selectedBackupSyncTarget;
			set => this.RaiseAndSetIfChanged(ref _selectedBackupSyncTarget, value);
		}

		public ReactiveCommand<BackupSyncTargetType, Unit> AddTarget { get; }

		public ReactiveCommand<Guid, Unit> RemoveTarget { get; }

		public ReactiveCommand<IBackupSyncTargetViewModel, Unit> SelectionChanged { get; }

		public ReactiveCommand<Unit, Unit> SaveConfigurations { get; }

		public ReactiveCommand<Unit, Unit> ResetTargets { get; }

		public ReactiveCommand<Unit, Unit> LoadTargets { get; }

		public BackupSyncSettingsPageViewModel()
		{
			_backupSyncSettings = Application.Instance.Container.GetInstance<IBackupSyncSettingsService>();

			AddTarget = ReactiveCommand.CreateFromTask<BackupSyncTargetType>(AddTargetAsync);
			RemoveTarget = ReactiveCommand.CreateFromTask<Guid>(RemoveTargetAsync);
			SelectionChanged = ReactiveCommand.CreateFromTask<IBackupSyncTargetViewModel>(SelectionChangedAsync);
			SaveConfigurations = ReactiveCommand.CreateFromTask(SaveTargetConfigurationsAsync);
			ResetTargets = ReactiveCommand.CreateFromTask(ResetTargetsAsync);
			LoadTargets = ReactiveCommand.CreateFromTask(LoadTargetsAsync);

			this.WhenAnyValue(x => x.SelectedBackupSyncTarget)
				.WhereNotNull()
				.InvokeCommand(SelectionChanged);

			var targets2 = _backupSyncSettings.Targets2;
			var op = targets2.Connect()
				.Transform<IBackupSyncTargetViewModel, IBackupSyncTarget, Guid>(x =>
					new LFSBackupSyncTargetViewModel {Target = (LfsBackupSyncTarget) x})
				.Clone(Targets)
				.Subscribe();

			_source.Connect()
				.Clone(Targets).Subscribe();
		}

		private Task AddTargetAsync(BackupSyncTargetType type)
		{
			_source.Add(new LFSBackupSyncTargetViewModel
			{
				Name = "Test1",
				Description = "Sync to folder test",
				TargetPath = "C:\\test",
				IsAssetsSyncEnabled = false,
				IsEnabled = false
			});

			return Task.CompletedTask;
		}

		private Task RemoveTargetAsync(Guid id) => _backupSyncSettings.RemoveTargetAsync(id);

		private Task SelectionChangedAsync(IBackupSyncTargetViewModel target)
		{
			switch (target.Type)
			{
				case BackupSyncTargetType.LocalFileStorage:
					_backupSyncTargetViewModel = target;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			return Task.CompletedTask;
		}

		private async Task SaveTargetConfigurationsAsync()
		{
			var a = Targets.AsParallel().Select(async x => await x.SaveConfiguration.Execute()).ToList();

			_source.Clear();

			foreach (var task in a)
			{
				var target = await task;
				await _backupSyncSettings.AddOrUpdateTargetAsync(target);
			}
		}

		private async Task ResetTargetsAsync()
		{
			_source.Clear();

			foreach (var target in Targets)
			{
				await target.ResetConfiguration.Execute();
			}
		}

		private Task LoadTargetsAsync() => _backupSyncSettings.LoadTargets();
	}

	public enum BackupSyncTargetType
	{
		LocalFileStorage
	}
}