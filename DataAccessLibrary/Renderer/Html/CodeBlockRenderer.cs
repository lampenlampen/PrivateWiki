using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using System;
using System.IO;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer.Html
{
	public class CodeBlockRenderer : HtmlRenderer
	{
		public override string RenderToHtml(IPageBlock block)
		{
			if (!(block is CodeBlock codeBlock)) throw new ArgumentException("Invalid Block Type", nameof(block));

			var renderer = new MarkdigHtmlRenderer(new StringWriter());
			renderer.EnsureLine();
			renderer.Write("<pre><code>");
			renderer.Write(codeBlock.Code);
			renderer.Write("</code></pre>");
			renderer.EnsureLine();
			renderer.Writer.Flush();

			return renderer.Writer.ToString();
		}

		public override void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer)
		{
			if (!(block is CodeBlock codeBlock)) throw new ArgumentException("Invalid Block Type", nameof(block));

			renderer.EnsureLine();
			renderer.Write("<pre><code>");
			renderer.Write(codeBlock.Code);
			renderer.Write("</code></pre>");
			renderer.EnsureLine();
		}
	}
}