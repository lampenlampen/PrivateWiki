using Markdig;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using StorageBackend.PageAST;
using StorageBackend.PageAST.Blocks;
using CodeBlock = StorageBackend.PageAST.Blocks.CodeBlock;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace StorageBackend.Test
{
	[TestClass]
	public class HtmlRendererTest
	{
		[TestMethod]
		public void RenderToHtmlTest()
		{
			var markdown = "Hallo *dies* ist ein _Markdown_ Text.\n\n```\npublic static void main(String[] args)\n{\n    System.out.println(\"Hello World\");\n}\n```\n\nHallo wie geht es dir?";
			var code = "public static void main(String[] args)\n{\n    System.out.println(\"Hello World\");\n}";
			var doc = Markdown.Parse(markdown);
			var markdownBlock = new MarkdownBlock(doc, markdown);
			var codeBlock = new CodeBlock(code, "java");

			Document document = new Document(new List<IPageBlock> { markdownBlock, codeBlock, markdownBlock });

			var htmlRenderer = new MarkdigHtmlRenderer(new StringWriter());
			Renderer.Html.HtmlRenderer.RenderToHtml2(document, htmlRenderer);

			htmlRenderer.Writer.Flush();
			var html = htmlRenderer.Writer.ToString();
		}
	}
}