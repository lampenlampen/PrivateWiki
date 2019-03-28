using System.IO;
using System.Runtime.InteropServices;
using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using JetBrains.Annotations;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DataAccessLibrary.Markdig
{
	public class HtmlRenderer
	{
		private global::Markdig.Renderers.HtmlRenderer _renderer;

		public HtmlRenderer(global::Markdig.Renderers.HtmlRenderer renderer)
		{
			_renderer = renderer;
		}

		public string RenderInlinesToHtml([NotNull] Inline inline)
		{
			while (inline != null)
			{
				_renderer.Write(inline);
				inline = inline.NextSibling;
			}

			_renderer.Writer.Flush();
			return _renderer.Writer.ToString();
		}

		public string RenderTextBlock([NotNull] TextBlock block)
		{
			var writer = (TextWriter) _renderer.Render(block.Content);
			writer.Flush();
			
			return writer.ToString();
		}
	}
}