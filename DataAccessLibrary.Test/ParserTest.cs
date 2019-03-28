using System.IO;
using DataAccessLibrary.Markdig;
using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessLibrary.Test
{
	[TestClass]
	public class ParserTest
	{
		[TestMethod]
		public void ParseTest()
		{
			var markdown = "Hallo *dies* ist ein __Markdown__ Text.\n\n# Headering 1";
			var doc = Parser.ParseMarkdown(markdown);

			var renderer = new HtmlRenderer(new global::Markdig.Renderers.HtmlRenderer(new StringWriter()));
			var html = renderer.RenderTextBlock((TextBlock) doc.Blocks[0]);
		}

		[TestMethod]
		public void ParseMarkdownTextTest()
		{
			var markdown = "Hallo *dies* ist ein __Markdown__ Text.";
			var inlines = Parser.ParseMarkdownText(markdown);
			
			var renderer = new HtmlRenderer(new global::Markdig.Renderers.HtmlRenderer(new StringWriter()));
			var html = renderer.RenderInlinesToHtml(inlines);
		}
	}
}