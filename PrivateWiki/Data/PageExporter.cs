using System;
using System.Threading.Tasks;
using Windows.Storage;
using PrivateWiki.Models.Pages;
using PrivateWiki.Renderer;

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