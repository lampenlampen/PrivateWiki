using System;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using PrivateWiki.DataModels;
using PrivateWiki.DataModels.Settings;
using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.LFSBackupService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public interface IBackupSyncTargetViewModel : INotifyPropertyChanged
	{
		public Guid Id { get; }

		public string Name { get; }

		public string TargetPath { get; }

		public string FontGlyph { get; }

		public bool IsEnabled { get; }

		public BackupSyncTargetType Type { get; }

		public ReactiveCommand<Unit, IBackupSyncTarget> SaveConfiguration { get; }

		public ReactiveCommand<Unit, Unit> ResetConfiguration { get; }
	}

	public class LFSBackupSyncTargetViewModel : ReactiveObject, IBackupSyncTargetViewModel
	{
		private readonly IFeatureFlagsService _featureFlags;
		private readonly ILFSBackupService _lfsBackup;

		public IFeatureFlagsService FeatureFlags => _featureFlags;

		private LfsBackupSyncTarget? _target;

		public LfsBackupSyncTarget? Target
		{
			get => _target;
			set => this.RaiseAndSetIfChanged(ref _target, value);
		}

		public BackupSyncTargetType Type { get; } = BackupSyncTargetType.LocalFileStorage;

		private Guid _id = Guid.NewGuid();

		public Guid Id
		{
			get => _id;
			set => this.RaiseAndSetIfChanged(ref _id, value);
		}

		private string? _name;

		public string? Name
		{
			get => _name;
			set => this.RaiseAndSetIfChanged(ref _name, value);
		}

		public string FontGlyph { get; } = "\uE1DF";

		private string? _description;

		public string? Description
		{
			get => _description;
			set => this.RaiseAndSetIfChanged(ref _description, value);
		}

		private string? _targetPath;

		public string? TargetPath
		{
			get => _targetPath;
			set => this.RaiseAndSetIfChanged(ref _targetPath, value);
		}

		private bool _isAssetsSyncEnabled = false;

		public bool IsAssetsSyncEnabled
		{
			get => _isAssetsSyncEnabled;
			set => this.RaiseAndSetIfChanged(ref _isAssetsSyncEnabled, value);
		}

		private bool _isEnabled = false;

		public bool IsEnabled
		{
			get => _isEnabled;
			set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
		}

		private SyncFrequency _frequency = SyncFrequency.Never;

		public SyncFrequency Frequency
		{
			get => _frequency;
			set => this.RaiseAndSetIfChanged(ref _frequency, value);
		}

		public ReactiveCommand<Unit, Unit> ExportContent { get; }

		public ReactiveCommand<Unit, Unit> CreateBackup { get; }

		public ReactiveCommand<Unit, IBackupSyncTarget> SaveConfiguration { get; }

		public ReactiveCommand<Unit, Unit> ResetConfiguration { get; }

		private ReactiveCommand<LfsBackupSyncTarget, Unit> LoadConfiguration { get; }

		public LFSBackupSyncTargetViewModel()
		{
			_featureFlags = ServiceLocator.Container.GetInstance<IFeatureFlagsService>();
			_lfsBackup = ServiceLocator.Container.GetInstance<ILFSBackupService>();

			ExportContent = ReactiveCommand.CreateFromTask(ExportContentAsync);
			CreateBackup = ReactiveCommand.CreateFromTask(CreateBackupAsync);
			SaveConfiguration = ReactiveCommand.CreateFromTask(SaveConfigurationAsync);
			ResetConfiguration = ReactiveCommand.CreateFromTask(ResetConfigurationAsync);
			LoadConfiguration = ReactiveCommand.CreateFromTask<LfsBackupSyncTarget>(LoadConfigurationAsync);

			this.WhenAnyValue(x => x.Target)
				.WhereNotNull()
				.InvokeCommand(LoadConfiguration);
		}

		private Task ExportContentAsync() =>
			Task.Run(async () =>
			{
				await _lfsBackup
					.ExportAsync(new LFSBackupServiceOptions(IsAssetsSyncEnabled, new Folder(TargetPath, "")))
					.ConfigureAwait(false);
			});

		private Task CreateBackupAsync() =>
			Task.Run(async () =>
				await _lfsBackup
					.CreateBackupAsync(new LFSBackupServiceOptions(IsAssetsSyncEnabled, new Folder(TargetPath, "")))
					.ConfigureAwait(false));

		private Task<IBackupSyncTarget> SaveConfigurationAsync()
		{
			// TODO SaveConfiguration

			return Task.FromResult<IBackupSyncTarget>(new LfsBackupSyncTarget(Id, Name, Description, TargetPath, Frequency, IsEnabled,
				IsAssetsSyncEnabled));
		}

		private Task ResetConfigurationAsync()
		{
			Name = _target.Name;
			Description = _target.Description;
			TargetPath = _target.TargetPath;
			Frequency = _target.Frequency;
			IsEnabled = _target.IsEnabled;
			IsAssetsSyncEnabled = _target.IsAssetsSyncEnabled;

			return Task.CompletedTask;
		}

		private Task LoadConfigurationAsync(LfsBackupSyncTarget target)
		{
			Id = target.Id;
			Name = target.Name;
			Description = target.Description;
			TargetPath = target.TargetPath;
			Frequency = target.Frequency;
			IsAssetsSyncEnabled = target.IsAssetsSyncEnabled;
			IsEnabled = target.IsEnabled;

			return Task.CompletedTask;
		}
	}
}