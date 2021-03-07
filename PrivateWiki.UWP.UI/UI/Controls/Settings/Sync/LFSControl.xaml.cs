using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls.Primitives;
using PrivateWiki.DataModels;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;

#nullable enable

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI.Controls.Settings.Sync
{
	public class LFSControlBase : ReactiveUserControl<LFSBackupSyncTargetViewModel> { }

	public sealed partial class LFSControl : LFSControlBase
	{
		public LFSControl()
		{
			this.InitializeComponent();

			this.WhenActivated(disposable =>
			{
				PickTargetFolderBtn.Events().Click
					.SelectMany(async x => await PickTargetFolderAsync())
					.WhereNotNull()
					.Select(x => x.Path)
					.ObserveOnDispatcher()
					.BindTo(ViewModel, vm => vm.TargetPath)
					.DisposeWith(disposable);

				this.OneWayBind(ViewModel,
						vm => vm.TargetPath,
						view => view.TargetPath.Text)
					.DisposeWith(disposable);


				this.Bind(ViewModel,
						vm => vm.IsAssetsSyncEnabled,
						view => view.IsAssetsSyncEnabledToogleSwitch.IsOn)
					.DisposeWith(disposable);

				// Disable actions when target not enabled
				this.OneWayBind(ViewModel,
						vm => vm.IsEnabled,
						view => view.CreateBackupBtn.IsEnabled)
					.DisposeWith(disposable);
				this.OneWayBind(ViewModel,
						vm => vm.IsEnabled,
						view => view.ExportContentBtn.IsEnabled)
					.DisposeWith(disposable);

				// Actions
				CreateBackupBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, vm => vm.CreateBackup)
					.DisposeWith(disposable);
				ExportContentBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, vm => vm.ExportContent)
					.DisposeWith(disposable);
			});
		}

		private async Task<Folder?> PickTargetFolderAsync()
		{
			var picker = new FolderPicker
			{
				ViewMode = PickerViewMode.List,
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			picker.FileTypeFilter.Add("*");

			var result = await picker.PickSingleFolderAsync();

			if (result != null)
			{
				var folder = new Folder(result.Path, result.Name);
				return folder;
			}

			return null;
		}
	}
}