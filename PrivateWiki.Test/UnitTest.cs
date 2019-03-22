using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using PrivateWiki.Data;

namespace PrivateWiki.Test
{
	[TestClass]
	public class UnitTest1
	{
		private TestContext testContextInstance;

		/// <summary>
		///  Gets or sets the test context which provides
		///  information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get { return testContextInstance; }
			set { testContextInstance = value; }
		}

		[TestMethod]
		public async void TestMethod1()
		{
		}

		[TestMethod]
		public void Test()
		{
			Assert.IsTrue(true);
		}
	}
}