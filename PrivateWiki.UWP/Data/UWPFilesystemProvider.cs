using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using PrivateWiki.DataModels;
using PrivateWiki.Services.FilesystemService;

namespace PrivateWiki.UWP.Data
{
	public class UWPFilesystemProvider : IFilesystemService
	{
		private readonly Dictionary<string, string> TokenStorage = new Dictionary<string, string>();

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
	}
}