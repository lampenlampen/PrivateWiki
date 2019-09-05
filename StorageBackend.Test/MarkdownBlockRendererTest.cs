using Markdig;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageBackend.PageAST.Blocks;
using StorageBackend.Renderer.Html;

namespace StorageBackend.Test
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

			var output = markdownBlockRenderer.RenderToHtml(new MarkdownBlock(doc, markdown));

			Assert.AreEqual(output, Markdown.ToHtml(markdown));
		}

		[TestMethod]
		public void RenderToHtmlTest2()
		{
			var markdown = "# Welcome to your Private Wiki\n\n## Get Started\n\nTo learn more about the syntax have a lock in the [Syntax](:syntax) page.\n\nTo view a preview article follow this [link](:test)";
			var doc = Markdown.Parse(markdown);

			var markdownBlockRenderer = new MarkdownBlockRenderer();

			var output = markdownBlockRenderer.RenderToHtml(new MarkdownBlock(doc, markdown));

			Assert.AreEqual(output, Markdown.ToHtml(markdown));
		}
	}
}