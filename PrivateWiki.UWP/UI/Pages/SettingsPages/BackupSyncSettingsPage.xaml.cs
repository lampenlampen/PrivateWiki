using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.UWP.UI.Controls.Settings.Sync;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class BackupSyncSettingsPage : Page, IViewFor<BackupSyncSettingsPageViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(BackupSyncSettingsPageViewModel), typeof(BackupSyncSettingsPage), new PropertyMetadata(null));

		public BackupSyncSettingsPageViewModel ViewModel
		{
			get => (BackupSyncSettingsPageViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (BackupSyncSettingsPageViewModel) value;
		}

		#endregion

		private IBackupSyncTargetViewModel? _selectedItem;

		public BackupSyncSettingsPage()
		{
			this.InitializeComponent();

			ViewModel = new BackupSyncSettingsPageViewModel();

			this.WhenActivated(disposable =>
			{
				this.OneWayBind(ViewModel,
						x => x.Targets,
						view => view.TargetList.ItemsSource)
					.DisposeWith(disposable);

				TargetList.Events().SelectionChanged
					.Select(_ => TargetList.SelectedItem as IBackupSyncTargetViewModel)
					.WhereNotNull()
					.InvokeCommand(ViewModel.SelectionChanged)
					.DisposeWith(disposable);

				ViewModel.OnDisplayBackupSyncTarget
					.Subscribe(DisplayBackupSyncTarget)
					.DisposeWith(disposable);
			});
		}

		private void DisplayBackupSyncTarget(IBackupSyncTargetViewModel vm)
		{
			BackupSyncTargetContent.Children.Clear();

			switch (vm.Type)
			{
				case BackupSyncTargetType.LocalFileStorage:
					var control = new LFSControl {ViewModel = (LFSBackupSyncTargetViewModel) vm};
					BackupSyncTargetContent.Children.Add(control);
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void AddLocalFileSystemTargetClick(object sender, RoutedEventArgs e)
		{
			ViewModel.AddTarget.Execute(BackupSyncTargetType.LocalFileStorage).Subscribe();
		}
	}
}