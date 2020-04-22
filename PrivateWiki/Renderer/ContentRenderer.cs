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
					case "text":
						return $"<pre>{page.Content}</pre>";
					default:
						return page.Content;
				}
			});
		}

		public Task<string> RenderContentAsync(string content, string contentType)
		{
			return Task.Run(() =>
			{
				switch (contentType.ToLower())
				{
					case "markdown":
						var renderer = new Markdig.Markdig();
						return renderer.ToHtmlCustom(renderer.Parse(content));
					case "html":
						return content;
					case "text":
						return $"<pre>{content}</pre>";
					default:
						return content;
				}
			});
		}
	}
}