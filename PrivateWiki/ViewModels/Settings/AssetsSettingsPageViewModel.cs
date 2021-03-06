using System.Reactive;
using System.Threading.Tasks;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationDataService;
using PrivateWiki.Services.FileExplorerService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class AssetsSettingsPageViewModel : ReactiveObject
	{
		private readonly IFileExplorerService _fileExplorerService;
		private readonly IApplicationDataService _applicationData;

		private Folder? _root = null;

		public Folder? Root
		{
			get => _root;
			set => this.RaiseAndSetIfChanged(ref _root, value);
		}

		private Folder? _selectedFolder = null;

		public Folder? SelectedFolder
		{
			get => _selectedFolder;
			set => this.RaiseAndSetIfChanged(ref _selectedFolder, value);
		}

		public ReactiveCommand<Folder?, Unit> OpenFolderInFileExplorer { get; }

		public ReactiveCommand<Unit, Unit> LoadDataFolder { get; }

		public AssetsSettingsPageViewModel()
		{
			_fileExplorerService = ServiceLocator.Container.GetInstance<IFileExplorerService>();
			_applicationData = ServiceLocator.Container.GetInstance<IApplicationDataService>();

			OpenFolderInFileExplorer = ReactiveCommand.CreateFromTask<Folder?>(OpenFolderInFileExplorerAsync);
			LoadDataFolder = ReactiveCommand.CreateFromTask(LoadTreeViewAsync);
		}

		private async Task OpenFolderInFileExplorerAsync(Folder? folder)
		{
			if (SelectedFolder == null)
			{
				await _fileExplorerService.ShowFolderAsync(_root);
			}
			else
			{
				await _fileExplorerService.ShowFolderAsync(SelectedFolder);
			}

			//await _fileExplorerService.ShowFolderAsync(SelectedFolder);
		}

		private async Task LoadTreeViewAsync()
		{
			Root = await _applicationData.GetDataFolderAsync();
		}
	}
}