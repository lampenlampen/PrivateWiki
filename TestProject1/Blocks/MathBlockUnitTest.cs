using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser.Blocks;

namespace TestProject1.Blocks
{
	[TestClass]
	public class MathBlockUnitTest : ParserTestBase
	{
		[TestMethod]
		[TestCategory("Parse - block")]
		public void MathBlockTest_1()
		{
			var markup = "\\frac{1}{2}";

			var block = MathBlock.Parse(markup, 0);

			Assert.AreEqual($"\\[{markup}\\]", block.ToString());
		}
	}
}