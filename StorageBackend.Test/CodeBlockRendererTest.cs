using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageBackend.PageAST.Blocks;
using StorageBackend.Renderer.Html;

namespace StorageBackend.Test
{
	[TestClass]
	public class CodeBlockRendererTest
	{
		[TestMethod]
		public void RenderToHtmlTest()
		{
			var markdown = "public static void main(String[] args)\n{\n    System.out.println(\"Hello World\");\n}";

			var codeBlock = new CodeBlock(markdown, "java");

			var html = new CodeBlockRenderer().RenderToHtml(codeBlock);
		}
	}
}