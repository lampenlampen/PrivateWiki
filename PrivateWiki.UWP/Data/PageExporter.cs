using System;
using System.Threading.Tasks;
using Windows.Storage;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.RenderingService;

namespace PrivateWiki.UWP.Data
{
	public class PageExporter
	{
		public PageExporter()
		{
		}

		public async Task<StorageFile> ExportPage(MarkdownPage page)
		{
			var contentRenderer = new ContentRenderer();
			var content = contentRenderer.RenderContentAsync(page.Content, ContentType.Markdown);

			var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{page.Link.Replace(':', '_')}.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(file, await content);

			return file;
		}

		public async Task<StorageFile> ExportPage(GenericPage page)
		{
			var renderer = new ContentRenderer();
			var data = renderer.RenderPageAsync(page);
			var file = ApplicationData.Current.TemporaryFolder.CreateFileAsync($"{page.Path.FullPath.Replace(':', '_')}.html", CreationCollisionOption.ReplaceExisting);

			await FileIO.WriteTextAsync(await file, await data);

			return await file;
		}
	}
}