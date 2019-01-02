using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;
using Parser.Exceptions;

namespace TestProject1.Blocks
{
	[TestClass]
	public class TableBlockUnitTest : ParserTestBase
	{
		[TestMethod]
		[TestCategory("Parse - block")]
		public void TableBlockTest_1()
		{
			var markup = "| Header 1 | Header 2 | Header 3 |\r\n|---|:---|---:|\r\n| Row 1 | Row 2 | Row 3 |";

			var block = TableBlock.Parse(markup.SplitIntoLines());

			Assert.AreEqual(markup, block.ToString());
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void TableBlockTest_2()
		{
			var markup = "| Header 1 | Header 2 | Header 3 |\r\n|---|:---|---:|\r\n| Row 1 | Row 2 | Row 3 |";

			var block = TableBlock.Parse(markup.SplitIntoLines());

			Assert.AreEqual(markup, block.ToString());
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void TableBlockTest_3()
		{
			var markup = "| Header 1 |\r\n|---:|\r\n| Row 1 |";

			var block = TableBlock.Parse(markup.SplitIntoLines());

			Assert.AreEqual(markup, block.ToString());
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void TableBlockTest_4()
		{
			var markup = "|  |\r\n|---:|\r\n|  |";

			var block = TableBlock.Parse(markup.SplitIntoLines());

			Assert.AreEqual(markup, block.ToString());
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void TableBlockTest_5()
		{
			var markup = "|  |\r\n| |\r\n|  |";

			try
			{
				TableBlock.Parse(markup.SplitIntoLines());
				Assert.Fail();
			}
			catch (TableBlockException e)
			{
				Console.WriteLine(e);
			}
		}

		[TestMethod]
		[TestCategory("Parse - block")]
		public void TableBlockTest_6()
		{
			var markup = "| h |\r\n|---:|\r\n|  |";

			var block = TableBlock.Parse(markup.SplitIntoLines());

			Assert.AreEqual(markup, block.ToString());
		}
	}
}