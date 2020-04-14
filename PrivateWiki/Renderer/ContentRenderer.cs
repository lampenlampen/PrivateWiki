using System.Diagnostics;
using System.Threading.Tasks;
using PrivateWiki.Models.Pages;

namespace PrivateWiki.Renderer
{
	public class ContentRenderer
	{
		public Task<string> RenderPageAsync(GenericPage page)
		{
			return Task.Run(() =>
			{
				Debug.Assert(page != null, nameof(page) + " != null");

				switch (page.ContentType.ToLower())
				{
					case "markdown":
						var renderer = new Markdig.Markdig();
						return renderer.ToHtmlCustom(renderer.Parse(page.Content));
					case "html":
						return page.Content;
					default:
						return page.Content;
				}
			});
		}
	}
}