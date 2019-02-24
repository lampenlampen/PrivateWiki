using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Services.TargetedContent;
using Windows.Storage;
using PrivateWiki.Markdig;
using StorageProvider;

namespace PrivateWiki.Data
{
	public class PageExporter
	{
		public PageExporter()
		{

		}

		public async Task<StorageFile> ExportPage(ContentPage page)
		{
			var parser = new MarkdigParser();
			var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{page.Id.Replace(':', '_')}.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(file, parser.ToHtmlString(page.Content));

			return file;
		}
	}
}