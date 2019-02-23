using System;
using System.Threading.Tasks;
using Windows.Storage;
using StorageProvider;

namespace PrivateWiki.Data
{
	public class MarkdownImport
	{
		private StorageFile File { get; set; }

		public async Task<ContentPage> ImportMarkdownFileAsync(StorageFile file)
		{
			File = file;

			var content = await ReadMarkdownFileAsync();

			return new ContentPage(file.DisplayName, content);
		}

		private async Task<string> ReadMarkdownFileAsync()
		{
			return await FileIO.ReadTextAsync(File);
		}
	}
}