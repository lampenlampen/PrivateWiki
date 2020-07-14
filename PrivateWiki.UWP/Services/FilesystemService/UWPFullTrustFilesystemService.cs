using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using PrivateWiki.DataModels;
using PrivateWiki.Services.FilesystemService;

namespace PrivateWiki.UWP.Services.FilesystemService
{
	public class UWPFullTrustFilesystemService : IFilesystemService
	{
		public async Task<File> WriteTextAsync(File file, string content)
		{
			var storageFile = await StorageFile.GetFileFromPathAsync(file.Path).AsTask().ConfigureAwait(false);

			await FileIO.WriteTextAsync(storageFile, content);

			return file;
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

			return new File(storageFile.Path);
		}

		public async Task<string> ReadTextAsync(File file)
		{
			var storageFile = await ToNative(file);

			return await FileIO.ReadTextAsync(storageFile);
		}

		private async Task<StorageFile> ToNative(File file)
		{
			var path = file.Path.Replace('\\', '/');

			var storageFile = await StorageApplicationPermissions.FutureAccessList.GetFileAsync(path);
			return storageFile;
		}

		private async Task<StorageFolder> ToNative(Folder folder)
		{
			var storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folder.Path);
			return storageFolder;
		}

		public async Task<Folder> GetDataFolder()
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Folder>> GetAllFolders(Folder root)
		{
			throw new NotImplementedException();
		}

		public async Task<Folder> CreateFolder(Folder folder, string folderName)
		{
			var storageFolder = await StorageFolder.GetFolderFromPathAsync(folder.Path).AsTask().ConfigureAwait(false);

			var newStorageFolder = await storageFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);

			return new Folder(newStorageFolder.Path, newStorageFolder.Name);
		}

		public async Task<File> CreateFile(Folder folder, string fileName)
		{
			var storageFolder = await StorageFolder.GetFolderFromPathAsync(folder.Path).AsTask().ConfigureAwait(false);
			var newStorageFile = await storageFolder.CreateFileAsync(fileName.Replace(':', '_'), CreationCollisionOption.OpenIfExists).AsTask().ConfigureAwait(false);

			return new File(newStorageFile.Path);
		}

		public async Task<Folder> Copy(Folder source, Folder target)
		{
			throw new NotImplementedException();
		}

		public async Task<File> Copy(File source, Folder target)
		{
			throw new NotImplementedException();
		}
	}
}