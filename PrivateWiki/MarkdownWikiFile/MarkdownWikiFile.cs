using System;
using System.IO;
using System.IO.Compression;
using Windows.Storage;
using PrivateWiki.Data;

namespace PrivateWiki.MarkdownWikiFile
{
	public class MarkdownWikiFile
	{
		public static async void saveToMarkdownWikiFile()
		{
			var tmpMarkdownWikiDir = await
				ApplicationData.Current.TemporaryFolder.CreateFolderAsync("MarkdownWikiTmp",
					CreationCollisionOption.ReplaceExisting);

			var pages = new ContentPageProvider().GetAllContentPages();

			foreach (var page in pages)
			{
				var name = page.Id;
				var file = await tmpMarkdownWikiDir.CreateFileAsync($"{name.Replace(":", "_")}.md");
				await File.WriteAllTextAsync(file.Path, page.Content);
			}

			var fileDest = await MediaAccess.PickMarkdownWikiFileAsync();

			ZipFile.CreateFromDirectory(tmpMarkdownWikiDir.Path, fileDest.Path);
		}
	}
}