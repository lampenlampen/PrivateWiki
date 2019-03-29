using System;
using System.IO;
using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer
{
	public class CodeBlockRenderer : HtmlRenderer
	{
		public override string RenderToHtml(IPageBlock block)
		{
			if(!(block is CodeBlock codeBlock)) throw new ArgumentException("Invalid Block Type", nameof(block));
			
			var renderer = new MarkdigHtmlRenderer(new StringWriter());
			renderer.EnsureLine();
			renderer.Write("<pre><code>");
			renderer.Write(codeBlock.Code);
			renderer.Write("</code></pre>");
			renderer.EnsureLine();
			renderer.Writer.Flush();

			return renderer.Writer.ToString();
		}

		public override void RenderToHtml(IPageBlock block, global::Markdig.Renderers.HtmlRenderer renderer)
		{
			if(!(block is CodeBlock codeBlock)) throw new ArgumentException("Invalid Block Type", nameof(block));

			renderer.EnsureLine();
			renderer.Write("<pre><code>");
			renderer.Write(codeBlock.Code);
			renderer.Write("</code></pre>");
			renderer.EnsureLine();
		}
	}
}