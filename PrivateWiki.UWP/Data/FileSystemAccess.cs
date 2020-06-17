using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;

#nullable enable

namespace PrivateWiki.UWP.Data
{
	[Obsolete]
	internal class FileSystemAccess
	{
		public static Task<StorageFolder> GetAssetsFolderAsync()
		{
			return Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets").AsTask();
		}

		public static async Task<StorageFolder> GetImageFolder()
		{
			var localFolder = ApplicationData.Current.LocalFolder;
			var mediaFolder = await localFolder.CreateFolderAsync("media", CreationCollisionOption.OpenIfExists);
			var imageFolder = await mediaFolder.CreateFolderAsync("images", CreationCollisionOption.OpenIfExists);
			return imageFolder;
		}

		public static async Task<StorageFile> PickImageFileAsync()
		{
			var picker = new FileOpenPicker
			{
				SuggestedStartLocation = PickerLocationId.PicturesLibrary,
				ViewMode = PickerViewMode.Thumbnail
			};

			picker.FileTypeFilter.Add(".jpg");
			picker.FileTypeFilter.Add(".png");
			picker.FileTypeFilter.Add(".gif");
			picker.FileTypeFilter.Add(".svg");
			picker.FileTypeFilter.Add(".ico");

			return await picker.PickSingleFileAsync();
		}

		public static async Task<StorageFolder> PickFolderAsync()
		{
			var picker = new FolderPicker
			{
				SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
				ViewMode = PickerViewMode.List
			};
			picker.FileTypeFilter.Add("*");

			return await picker.PickSingleFolderAsync();
		}
	}
}