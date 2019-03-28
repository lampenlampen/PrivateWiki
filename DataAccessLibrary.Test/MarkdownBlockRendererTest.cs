using DataAccessLibrary.PageAST.Blocks;
using DataAccessLibrary.Renderer;
using Markdig;
using Markdig.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessLibrary.Test
{
	[TestClass]
	public class MarkdownBlockRendererTest
	{
		[TestMethod]
		public void RenderToHtmlTest()
		{
			var markdown = "Hallo *dies* ist ein _Markdown_ Text.";
			var doc = Markdown.Parse(markdown);
 			
			var markdownBlockRenderer = new MarkdownBlockRenderer();

			var output = markdownBlockRenderer.RenderToHtml(new MarkdownBlock(doc, SourceSpan.Empty));
			
			Assert.AreEqual(output, Markdown.ToHtml(markdown));
		}
	}
}