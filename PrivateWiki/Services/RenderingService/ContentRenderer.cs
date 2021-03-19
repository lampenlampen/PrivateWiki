using System;
using System.Diagnostics;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Rendering.Markdown;

namespace PrivateWiki.Services.RenderingService
{
	public class ContentRenderer
	{
		private readonly Rendering.Markdown.Markdig.Markdig _markdig;

		public ContentRenderer(Rendering.Markdown.Markdig.Markdig markdig)
		{
			_markdig = markdig;
		}

		[Obsolete]
		public ContentRenderer()
		{
			_markdig = ServiceLocator.Container.GetInstance<Rendering.Markdown.Markdig.Markdig>();
		}

		public string RenderPage(GenericPage page)
		{
			switch (page.ContentType.MimeType)
			{
				case "text/markdown":
					var renderer = new MarkdownRenderer(_markdig);
					return renderer.RenderToHtml(page.Content);
				case "text/html":
					return page.Content;
				case "text/plain":
					return $"<pre>{page.Content}</pre>";
				default:
					return page.Content;
			}
		}

		public Task<string> RenderPageAsync(GenericPage page)
		{
			return Task.Run(async () =>
			{
				Debug.Assert(page != null, nameof(page) + " != null");

				switch (page.ContentType.MimeType)
				{
					case "text/markdown":
						var renderer = new MarkdownRenderer(_markdig);
						return await renderer.RenderToHtmlAsync(page.Content);
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
			return Task.Run(async () =>
			{
				switch (contentType.ToLower())
				{
					case "markdown":
						var renderer = new MarkdownRenderer(_markdig);
						return await renderer.RenderToHtmlAsync(content);
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
			return Task.Run(async () =>
			{
				switch (contentType.MimeType)
				{
					case "text/markdown":
						var renderer = new MarkdownRenderer(_markdig);
						return await renderer.RenderToHtmlAsync(content);
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