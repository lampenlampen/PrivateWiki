using System;
using System.Threading.Tasks;
using Windows.Storage;
using Models.Pages;
using PrivateWiki.Markdig;
using StorageBackend;

namespace PrivateWiki.Data
{
	public class PageExporter
	{
		public PageExporter()
		{

		}

		public async Task<StorageFile> ExportPage(MarkdownPage page)
		{
			var parser = new Markdig.Markdig();
			var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{page.Link.Replace(':', '_')}.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(file, await parser.ToHtmlString(page.Content));

			return file;
		}
	}
}