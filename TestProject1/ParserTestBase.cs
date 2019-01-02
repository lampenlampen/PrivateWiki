using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject1
{
	[TestClass]
	public class ParserTestBase
	{
		internal static string TrimNewLinesAndWhitespaces(string text)
		{
			return text.Trim('\n', '\r', ' ');
		}

		[TestMethod]
		public void TrimNewLinesTest1()
		{
			const string actual = " \nHallo\nHuhu\n ";
			const string expected = "Hallo\nHuhu";

			Assert.AreEqual(TrimNewLinesAndWhitespaces(actual), expected);
		}

		[TestMethod]
		public void TrimNewLinesTest2()
		{
			const string actual = "\rHallo\rHuhu\r";
			const string expected = "Hallo\rHuhu";

			Assert.AreEqual(TrimNewLinesAndWhitespaces(actual), expected);
		}

		[TestMethod]
		public void TrimNewLinesTest3()
		{
			const string actual = "\n Hallo\nHuhu\n ";
			const string expected = "Hallo\nHuhu";

			Assert.AreEqual(TrimNewLinesAndWhitespaces(actual), expected);
		}

		[TestMethod]
		public void TrimNewLinesTest4()
		{
			const string actual = "\r\n Hallo\r\nHuhu\r\n ";
			const string expected = "Hallo\r\nHuhu";

			Assert.AreEqual(TrimNewLinesAndWhitespaces(actual), expected);
		}
	}
}