using DataAccessLibrary;
using PrivateWiki.Markdig;
using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Services.TargetedContent;
using Windows.Storage;

namespace PrivateWiki.Data
{
	public class PageExporter
	{
		public PageExporter()
		{

		}

		public async Task<StorageFile> ExportPage(PageModel page)
		{
			var parser = new MarkdigParser();
			var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{page.Link.Replace(':', '_')}.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(file, await parser.ToHtmlString(page.Content));

			return file;
		}
	}
}