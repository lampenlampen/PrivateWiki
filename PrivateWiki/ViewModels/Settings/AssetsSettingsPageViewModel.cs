using System.Collections.Generic;
using System.Reactive;
using System.Threading;
using System.Threading.Tasks;
using PrivateWiki.DataModels;
using PrivateWiki.Services.ApplicationDataService;
using PrivateWiki.Services.FileExplorerService;
using PrivateWiki.Services.FilesystemService;
using ReactiveUI;
using SimpleInjector;

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

		public ReactiveCommand<Unit, Unit> OpenFolderInFileExplorer { get; }

		public ReactiveCommand<Unit, Unit> LoadDataFolder { get; }

		public AssetsSettingsPageViewModel()
		{
			_fileExplorerService = Application.Instance.Container.GetInstance<IFileExplorerService>();
			_applicationData = Application.Instance.Container.GetInstance<IApplicationDataService>();

			OpenFolderInFileExplorer = ReactiveCommand.CreateFromTask(OpenFolderInFileExplorerAsync);
			LoadDataFolder = ReactiveCommand.CreateFromTask(LoadTreeViewAsync);
		}

		private async Task OpenFolderInFileExplorerAsync()
		{
			var folder = Root;

			if (folder != null) await _fileExplorerService.ShowFolderAsync(folder);
		}

		private async Task LoadTreeViewAsync()
		{
			Root = await _applicationData.GetDataFolder();
		}
	}
}