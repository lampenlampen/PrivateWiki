using System;
using System.Threading.Tasks;
using Windows.Storage;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.RenderingService;

namespace PrivateWiki.UWP.UI.Data
{
	[Obsolete]
	public class PageExporter
	{
		[Obsolete]
		public async Task<StorageFile> ExportPage(MarkdownPage page)
		{
			var contentRenderer = new ContentRenderer();
			var content = contentRenderer.RenderContentAsync(page.Content, ContentType.Markdown);

			var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{page.Link.Replace(':', '_')}.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(file, await content);

			return file;
		}
	}
}