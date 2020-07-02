using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using PrivateWiki.DataModels;
using PrivateWiki.Services.FilesystemService;
using PrivateWiki.Services.KeyValueCaches;
using File = PrivateWiki.DataModels.File;
using Guard = Microsoft.Toolkit.Diagnostics.Guard;

namespace PrivateWiki.UWP.Services.FilesystemService
{
	public class FilesystemService : IFilesystemService
	{
		private static readonly Dictionary<string, string> TokenStorage = new Dictionary<string, string>();
		private readonly IPersistentKeyValueCache _cache;

		public async Task WriteTextAsync(File file, string content)
		{
			var storageFile = await ToNative(file);

			await FileIO.WriteTextAsync(storageFile, content);
		}

		public async Task<File> PickFile(string fileExtension)
		{
			var picker = new FileOpenPicker
			{
				ViewMode = PickerViewMode.List,
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary
			};

			picker.FileTypeFilter.Add(fileExtension);

			var storageFile = await picker.PickSingleFileAsync();

			var token = StorageApplicationPermissions.FutureAccessList.Add(storageFile);

			TokenStorage.Add(storageFile.Path, token);

			return new File(storageFile.Path);
		}

		public async Task<string> ReadTextAsync(File file)
		{
			var storageFile = await ToNative(file);

			return await FileIO.ReadTextAsync(storageFile);
		}

		private async Task<StorageFile> ToNative(File file)
		{
			var token = TokenStorage[file.Path];

			var storageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(token);
			return storageFile;
		}

		private async Task<StorageFolder> ToNative(Folder folder)
		{
			var token = TokenStorage[folder.Path];

			var storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(token);
			return storageFolder;
		}
		
		public async Task<Folder> GetDataFolder()
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Folder>> GetAllFolders(Folder root)
		{
			Guard.IsNotNull(root, nameof(root));
			
			var storageFolder = await ToNative(root).ConfigureAwait(false);

			var storageFolders = await storageFolder.GetFoldersAsync();

			foreach (var folder in storageFolders)
			{
				
			}
			
			return new List<Folder>();
		}
	}
}