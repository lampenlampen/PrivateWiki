using System.IO;
using DataAccessLibrary.Renderer;
using Markdig;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Test
{
	[TestClass]
	public class InlineRendererTest
	{
		[TestMethod]
		public void RenderToHtmlTest()
		{
			var markdown = "Hallo *dies* ist ein _Markdown_ Text.";
			var inlineRenderer = new InlineRenderer();
			var doc = Markdown.Parse(markdown);
			var inlines = (doc[0] as LeafBlock).Inline;
			
			var html = inlineRenderer.RenderToHtml(inlines);

			var markdigRenderer = new MarkdigHtmlRenderer(new StringWriter());
			
			inlineRenderer.RenderToHtml(inlines, markdigRenderer);
			
			markdigRenderer.Writer.Flush();
			var html2 = markdigRenderer.Writer.ToString();
			
			Assert.AreEqual(html, html2);
		}
	}
}