using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser.Blocks;

namespace TestProject1.Blocks
{
	[TestClass]
	public class HeaderBlockUnitTest : ParserTestBase
	{
		[TestMethod]
		public void HeaderBlockTest_1()
		{
			const int headingLevel = 3;
			const string headingText = "Hallo";

			var headingBuilder = new StringBuilder();

			for (var i = 0; i < headingLevel; i++) headingBuilder.Append("#");

			headingBuilder.Append(" ").Append(headingText);

			var testText = headingBuilder.ToString();

			var heading = HeaderBlock.Parse(testText);

			Console.WriteLine(heading.HeadingText);
			Console.WriteLine(heading.Level);
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void HeaderBlockTest_2()
		{
			const string markup = "# Hallo";

			var block = HeaderBlock.Parse(markup);

			Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(block.ToString()));
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void HeaderBlockTest_3()
		{
			const string markup = "### Hallo Huhu";

			var block = HeaderBlock.Parse(markup);

			Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(block.ToString()));
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		[ExpectedException(typeof(ArgumentException), "Heading Level greater 5 are not supported!")]
		public void HeaderBlockTest_4()
		{
			const string markup = "###### Hallo";

			var block = HeaderBlock.Parse(markup);

			Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(block.ToString()));
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void HeaderBlockTest_5()
		{
			const string markup = "##### Hallo # Huhu";

			var block = HeaderBlock.Parse(markup);

			Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(block.ToString()));
		}
	}
}