using DataAccessLibrary.PageAST.Blocks;
using DataAccessLibrary.Renderer;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataAccessLibrary.Test
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