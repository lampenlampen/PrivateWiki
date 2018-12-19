using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace PrivateWiki.Data
{
	internal static class FileSystemAccess
	{
		private const string BackupFolderLocation = "BackupFolderLocation";

		public static async Task<bool> SaveToFolder()
		{
			var result = await ShowDialog();
			var folder = await MediaAccess.PickFolderAsync();

			if (result == ContentDialogResult.None) return false;


			var pages = new ContentPageProvider().GetAllContentPages();
			Debug.WriteLine($"Pages: {pages.Count}");

			foreach (var page in pages)
			{
				var file = await folder.CreateFileAsync($"{page.Id}.md", CreationCollisionOption.ReplaceExisting);
				using (var stream = await file.OpenStreamForWriteAsync())
				{
					using (var writer = new StreamWriter(stream))
					{
						await writer.WriteAsync(page.Content);
					}
				}
			}

			return true;
		}

		private static async Task<ContentDialogResult> ShowDialog()
		{
			var dialog = new ContentDialog
			{
				Title = "Export Pages",
				Content = "All pages will be exported. Please select a folder, where the files will be saved to.",
				CloseButtonText = "Cancel",
				PrimaryButtonText = "Select Folder",
				DefaultButton = ContentDialogButton.Close
			};

			return await dialog.ShowAsync();
		}
	}
}