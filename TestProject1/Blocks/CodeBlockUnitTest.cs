using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1.Blocks
{
	[TestClass]
	public class CodeBlockUnitTest : ParserTestBase
	{
		[TestMethod]
		public void CodeBlockTest_1()
		{
			var markup = "public static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld!\");\r\n}";
			var codeLanguage = "java";
			var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

			var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CodeBlockTest_2()
		{
			var markup = "public static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld!\");\r\n}";

			var codeLanguage = "markdown";
			var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

			var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CodeBlockTest_3()
		{
			var markup = "public static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld!\");\r\n}";
			var codeLanguage = "";
			var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

			var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CodeBlockTest_4()
		{
			var markup = "";
			var codeLanguage = "java";
			var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

			var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void CodeBlockTest_5()
		{
			// TODO Empty Codeblock
			var markup = "";
			var codeLanguage = "";
			var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

			var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

			Assert.AreEqual(expected, actual);
		}
	}
}