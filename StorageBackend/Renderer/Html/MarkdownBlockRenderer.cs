using System;
using System.IO;
using StorageBackend.PageAST;
using StorageBackend.PageAST.Blocks;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace StorageBackend.Renderer.Html
{
	public class MarkdownBlockRenderer : HtmlRenderer
	{
		public override string RenderToHtml(IPageBlock block)
		{
			if (!(block is MarkdownBlock markdownBlock)) throw new ArgumentException("Invalid Block", nameof(block));

			var renderer = new MarkdigHtmlRenderer(new StringWriter());

			renderer.Write(markdownBlock.Content);

			renderer.Writer.Flush();
			return renderer.Writer.ToString();
		}

		public override void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer)
		{
			if (!(block is MarkdownBlock markdownBlock)) throw new ArgumentException("Invalid Block", nameof(block));

			renderer.Write(markdownBlock.Content);
		}
	}
}