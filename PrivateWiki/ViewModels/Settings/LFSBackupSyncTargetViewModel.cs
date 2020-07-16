using System.Reactive;
using System.Threading.Tasks;
using PrivateWiki.DataModels;
using PrivateWiki.Services.AppSettingsService.FeatureFlagsService;
using PrivateWiki.Services.LFSBackupService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public interface IBackupSyncTargetViewModel
	{
		public string Name { get; }

		public string TargetPath { get; }

		public string FontGlyph { get; }

		public bool IsEnabled { get; }

		public BackupSyncTargetType Type { get; }

		public ReactiveCommand<Unit, Unit> SaveConfiguration { get; }
	}

	public class LFSBackupSyncTargetViewModel : ReactiveObject, IBackupSyncTargetViewModel
	{
		public IFeatureFlagsService FeatureFlags { get; }

		private readonly ILFSBackupService _lfsBackup;

		public BackupSyncTargetType Type { get; } = BackupSyncTargetType.LocalFileStorage;

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

		public ReactiveCommand<Unit, Unit> SaveConfiguration { get; }

		public LFSBackupSyncTargetViewModel()
		{
			FeatureFlags = Application.Instance.AppSettings.FeatureFlags;
			_lfsBackup = Application.Instance.Container.GetInstance<ILFSBackupService>();

			ExportContent = ReactiveCommand.CreateFromTask(ExportContentAsync);
			CreateBackup = ReactiveCommand.CreateFromTask(CreateBackupAsync);
			SaveConfiguration = ReactiveCommand.CreateFromTask(SaveConfigurationAsync);
		}

		private Task ExportContentAsync() =>
			Task.Run(async () => { await _lfsBackup.ExportAsync(new LFSBackupServiceOptions(IsAssetsSyncEnabled, new Folder(TargetPath, ""))).ConfigureAwait(false); });

		private Task CreateBackupAsync() =>
			Task.Run(async () => await _lfsBackup.CreateBackupAsync(new LFSBackupServiceOptions(IsAssetsSyncEnabled, new Folder(TargetPath, ""))).ConfigureAwait(false));

		private async Task SaveConfigurationAsync()
		{
			Application.Instance.GlobalNotificationManager.ShowNotImplementedNotification();
		}
	}
}