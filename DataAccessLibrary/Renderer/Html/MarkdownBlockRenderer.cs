using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using System;
using System.IO;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer.Html
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