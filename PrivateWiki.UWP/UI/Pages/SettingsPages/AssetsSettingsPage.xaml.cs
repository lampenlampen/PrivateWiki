using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using JetBrains.Annotations;
using PrivateWiki.DataModels;
using PrivateWiki.UWP.Services.FilesystemService;
using PrivateWiki.UWP.UI.Events;
using PrivateWiki.UWP.Utilities.ExtensionFunctions;
using PrivateWiki.ViewModels.Settings;
using ReactiveUI;
using muxc = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class AssetsSettingsPage : IViewFor<AssetsSettingsPageViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(AssetsSettingsPageViewModel), typeof(AssetsSettingsPage), new PropertyMetadata(null));

		public AssetsSettingsPageViewModel ViewModel
		{
			get => (AssetsSettingsPageViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (AssetsSettingsPageViewModel) value;
		}

		#endregion

		internal ObservableCollection<StorageFileWrapperForThumbnailProperty> Files2 = new ObservableCollection<StorageFileWrapperForThumbnailProperty>();

		public AssetsSettingsPage()
		{
			this.InitializeComponent();
			ViewModel = new AssetsSettingsPageViewModel();

			this.WhenActivated(disposable =>
			{
				ViewModel.LoadDataFolder.Execute().Subscribe();

				TreeViewMedia.Events().ItemInvoked
					.Select(x => ((StorageFolder) ((muxc.TreeViewNode) (x.InvokedItem)).Content).ToFolder())
					.BindTo(ViewModel, vm => vm.SelectedFolder)
					.DisposeWith(disposable);

				TreeViewMedia.Events().Expanding
					.Where(x => x.Node.HasUnrealizedChildren)
					.SelectMany(async x => await FillTreeViewNode(x.Node))
					.Subscribe()
					.DisposeWith(disposable);

				TreeViewMedia.Events().Collapsed
					.Subscribe(x =>
					{
						x.Node.Children.Clear();
						x.Node.HasUnrealizedChildren = true;
					})
					.DisposeWith(disposable);

				OpenInExplorerText.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel, x => x.OpenFolderInFileExplorer)
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.Root)
					.WhereNotNull()
					.SelectMany(async x => await LoadTreeView(x))
					.Subscribe()
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.SelectedFolder)
					.WhereNotNull()
					.Select(x => x.Path)
					.BindTo(ClickedItemPath, x => x.Text)
					.DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.SelectedFolder)
					.WhereNotNull()
					.SelectMany(async x => await FillDataGrid(x))
					.Subscribe()
					.DisposeWith(disposable);
			});
		}

		private async Task<Unit> LoadTreeView(Folder rootFolder)
		{
			var result = await rootFolder.ToNativeAsync();

			if (result.IsFailed)
			{
				return default;
			}

			var root = result.Value;

			var folders = await root.GetFoldersAsync();

			foreach (var folder in folders)
			{
				var hasSubfolders = await folder.HasSubfolders();

				var node = new muxc.TreeViewNode
				{
					Content = folder,
					HasUnrealizedChildren = hasSubfolders
				};

				TreeViewMedia.RootNodes.Add(node);
			}

			return Unit.Default;
		}

		private async Task<Unit> FillTreeViewNode(muxc.TreeViewNode node)
		{
			if (!node.HasUnrealizedChildren) return Unit.Default;

			var root = (StorageFolder) node.Content;

			var childFolders = await root.GetFoldersAsync();

			foreach (var folder in childFolders)
			{
				var hasSubfolders = await folder.HasSubfolders();

				var childNode = new muxc.TreeViewNode
				{
					Content = folder,
					HasUnrealizedChildren = hasSubfolders
				};

				node.Children.Add(childNode);
			}

			node.HasUnrealizedChildren = false;

			return Unit.Default;
		}

		private async Task<Unit> FillDataGrid(Folder folder)
		{
			Files2.Clear();

			await Task.Run(async () =>
				{
					var result = await folder.ToNativeAsync().ConfigureAwait(false);

					if (result.IsFailed)
					{
						// TODO
						return;
					}

					var files = await result.Value.GetFilesAsync().AsTask().ConfigureAwait(false);

					files
						.AsParallel()
						.Select(x => new StorageFileWrapperForThumbnailProperty(x))
						.RunOnUIThread(x => Files2.Add(x));
				}
			).ConfigureAwait(true);

			return Unit.Default;
		}
	}

	internal class StorageFileWrapperForThumbnailProperty : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		private readonly StorageFile _file;

		public string Name => _file.Name;

		public string Type => _file.DisplayType;

		private StorageItemThumbnail? _thumbnail;

		public StorageItemThumbnail? Thumbnail
		{
			get => _thumbnail;
			set
			{
				_thumbnail = value;
				OnPropertyChanged(nameof(Thumbnail));
			}
		}

		public StorageFileWrapperForThumbnailProperty(StorageFile file)
		{
			_file = file;

			Task.Run(async () => await Init().ConfigureAwait(false));
		}

		private async Task Init()
		{
			var thumbnail = await _file.GetThumbnailAsync(ThumbnailMode.ListView);

			await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { Thumbnail = thumbnail; });
		}
	}
}