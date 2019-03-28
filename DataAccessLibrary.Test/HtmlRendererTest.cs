using System.IO;
using DataAccessLibrary.Markdig;
using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using Markdig;
using Markdig.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Test
{
	[TestClass]
	public class HtmlRendererTest
	{
		[TestMethod]
		public void RendererInlinesToHtmlTest()
		{
			var markdown = "Hallo *dies* ist ein _Markdown_ Text.";
			var doc = Markdown.Parse(markdown);
			var inlines = (doc[0] as LeafBlock).Inline;

			var renderer = new HtmlRenderer(new MarkdigHtmlRenderer(new StringWriter()));
			var a = renderer.RenderInlinesToHtml(inlines);
		}

		[TestMethod]
		public void RenderTextBlockTest()
		{
			var markdown = "Hallo *dies* ist ein _Markdown_ Text.";
			var paragraphBlock = (ParagraphBlock) Markdown.Parse(markdown)[0];
			var textBlock = new TextBlock(markdown, paragraphBlock);

			var renderer = new HtmlRenderer(new MarkdigHtmlRenderer(new StringWriter()));
			var html = renderer.RenderTextBlock(textBlock);
		}
	}
}