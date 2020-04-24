using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PrivateWiki.Data;
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

				switch (page.ContentType.MimeType)
				{
					case "text/markdown":
						var renderer = new Markdig.Markdig();
						return renderer.ToHtmlCustom(renderer.Parse(page.Content));
					case "text/html":
						return page.Content;
					case "text/plain":
						return $"<pre>{page.Content}</pre>";
					default:
						return page.Content;
				}
			});
		}

		[Obsolete]
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

		public Task<string> RenderContentAsync(string content, ContentType contentType)
		{
			return Task.Run(() =>
			{
				switch (contentType.MimeType)
				{
					case "text/markdown":
						var renderer = new Markdig.Markdig();
						return renderer.ToHtmlCustom(renderer.Parse(content));
					case "text/html":
						return content;
					case "text/plain":
						return $"<pre>{content}</pre>";
					default:
						return content;
				}
			});
		}
	}
}