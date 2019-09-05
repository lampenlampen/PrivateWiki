using Markdig.Syntax.Inlines;
using System.IO;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace StorageBackend.Renderer.Html
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