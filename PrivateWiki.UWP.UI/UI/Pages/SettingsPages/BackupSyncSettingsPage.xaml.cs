using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using PrivateWiki.UWP.UI.UI.Controls.Settings.Sync;
using PrivateWiki.UWP.UI.Utilities.ExtensionFunctions;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages.SettingsPages
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
					.BindTo(ViewModel, vm => vm.SelectedBackupSyncTarget)
					.DisposeWith(disposable);

				DeleteTargetBtn.Events().Click
					.Select(_ => ViewModel.SelectedBackupSyncTarget)
					.WhereNotNull()
					.Select(x => x.Id)
					.InvokeCommand(ViewModel, x => x.RemoveTarget)
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.SelectedBackupSyncTarget)
					.Subscribe(SelectedTargetChanges)
					.DisposeWith(disposable);

				Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
						handler => Header.ApplyClick += handler,
						handler => Header.ApplyClick -= handler)
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, x => x.SaveConfigurations)
					.DisposeWith(disposable);

				Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
						handler => Header.ResetClick += handler,
						handler => Header.ResetClick -= handler)
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, x => x.ResetTargets)
					.DisposeWith(disposable);

				ViewModel.LoadTargets.Execute().Subscribe().DisposeWith(disposable);
			});
		}

		private void SelectedTargetChanges(IBackupSyncTargetViewModel? vm)
		{
			BackupSyncTargetContent.Children.Clear();

			if (vm != null)
			{
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
			else
			{
				BackupSyncTargetContent.Children.Add(new NothingSelectedControl());
			}
		}

		private void AddLocalFileSystemTargetClick(object sender, RoutedEventArgs e)
		{
			ViewModel.AddTarget.Execute(BackupSyncTargetType.LocalFileStorage).Subscribe();
		}

		private async void DeleteTargetClick(object sender, RoutedEventArgs e)
		{
			ListViewItem? item = ((DependencyObject) sender).FindParent<ListViewItem>();

			var target = (IBackupSyncTargetViewModel) item.Content;

			await ViewModel.RemoveTarget.Execute();
		}
	}
}