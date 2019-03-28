using System.IO;
using DataAccessLibrary.PageAST;
using Markdig.Syntax.Inlines;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer
{
	public class InlineRenderer
	{
		public string RenderToHtml(Inline inline)
		{
			var renderer = new MarkdigHtmlRenderer(new StringWriter());

			while (inline != null)
			{
				renderer.Write(inline);
				inline = inline.NextSibling;
			}

			renderer.Writer.Flush();
			return renderer.Writer.ToString();
		}

		public void RenderToHtml(Inline inline, MarkdigHtmlRenderer renderer)
		{
			while (inline != null)
			{
				renderer.Write(inline);
				inline = inline.NextSibling;
			}
		}
	}
}