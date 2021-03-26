using System.Collections.Generic;
using System.Linq;
using PrivateWiki.Core;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Rendering.Markdown;
using PrivateWiki.Rendering.Markdown.Markdig;
using Markdig = PrivateWiki.Rendering.Markdown.Markdig.Markdig;

namespace PrivateWiki.Services.ContentRenderer
{
	public interface IPageRenderer
	{
		
		public ContentType ContentType { get; }
		public string Render(string page);
	}

	public class RenderQueryHandler : IQueryHandler<GetRenderResult, RenderResult>
	{
		private IEnumerable<IPageRenderer> _pageRenderers;

		public RenderQueryHandler(IEnumerable<IPageRenderer> pageRenderers)
		{
			_pageRenderers = pageRenderers;
		}

		public RenderResult Handle(GetRenderResult query)
		{
			var page = query.Page;

			var html = _pageRenderers.First(renderer => renderer.ContentType.Equals(page.ContentType))
				.Render(page.Content);

			return new RenderResult(html);
		}
	}

	public class GetRenderResult : IQuery<RenderResult>
	{
		public GenericPage Page { get; }

		public GetRenderResult(GenericPage page)
		{
			Page = page;
		}
	}

	public class RenderResult
	{
		public string Html { get; }

		public RenderResult(string html)
		{
			Html = html;
		}
	}
	
	public class MarkdownRenderer : IPageRenderer
	{
		private IMarkdownRenderer _renderer;

		public MarkdownRenderer(IMarkdownRenderer renderer)
		{
			_renderer = renderer;
		}
		public ContentType ContentType => ContentType.Markdown;
		public string Render(string page)
		{
			return _renderer.Render(page);
		}
	}

	public interface IMarkdownRenderer : IPageRenderer
	{
		
	}
	
	public class DefaultRenderer : IPageRenderer, IMarkdownRenderer
	{
		public ContentType ContentType => ContentType.Text;
		public string Render(string page)
		{
			return $"<pre>{page}</pre>";
		}
	}
}