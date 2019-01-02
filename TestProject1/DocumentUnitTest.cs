using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace TestProject1
{
	[TestClass]
	public class DocumentUnitTest : ParserTestBase
	{
		[TestMethod]
		public void DocumentTest_1()
		{
			var markup =
				"# Header 1\r\n---\r\n| 1 | 2 | 3 |\r\n|---:|:---|:---:|\r\n| right | left | mid |\r\n\r\n´´´ java\r\npublic static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld\");\r\n}\r\n```";

			var document = Document.Parse(markup);

			Console.Out.WriteLine(document.ToString());

			Assert.AreEqual(markup, document.ToString());
		}


		[TestMethod]
		public void DocumentTest_2()
		{
			var markup = "# Header 1\r\n---\r\nHallo wie geht es dir?\r\n\r\n```\r\npublic class TestClass{}\r\n```";

			var document = Document.Parse(markup);

			Console.Out.WriteLine(document.ToString());

			Assert.AreEqual(markup, document.ToString());
		}
	}
}