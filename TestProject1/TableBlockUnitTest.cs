using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace TestProject1
{
    [TestClass]
    public class TableBlockUnitTest : ParserTestBase
    {
        [TestMethod]
        [TestCategory("Parse - block")]
        public void TableBlockTest_1()
        {
            const string markup = "|Header 1 | Header 2 | Header 3 |\n" +
                                  "|---|:---|---:|\n" +
                                  "| Row 1 | Row 2 | Row 3 |";

            var document = Document.Parse(markup);
            
            Console.WriteLine(document.ToString());

            Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(document.ToString()));
        }
    }
}