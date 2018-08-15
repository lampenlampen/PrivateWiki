using System;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1
{
    [TestClass]
    public class HeaderBlockUnitTest : ParserTestBase
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int headingLevel = 3;
            const string headingText = "Hallo";

            var headingBuilder = new StringBuilder();

            for (var i = 0; i < headingLevel; i++)
            {
                headingBuilder.Append("#");
            }

            headingBuilder.Append(" ").Append(headingText);

            var testText = headingBuilder.ToString();

            var heading = Document.Parse(testText).Dom.First();

            System.Console.WriteLine(((HeaderBlock) heading).HeadingText);
            System.Console.WriteLine(((HeaderBlock) heading).Level);
        }

        [TestMethod]
        [TestCategory("Parse - block")]
        public void HeaderBlockTest_1()
        {
            const string markup = "# Hallo";

            var document = Document.Parse(markup);
            
            Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(document.ToString()));
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HeaderBlockTest_2()
        {
            const string markup = "### Hallo Huhu";

            var document = Document.Parse(markup);

            Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(document.ToString()));
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        [ExpectedException(typeof(ArgumentException), "Heading Level greater 5 are not supported!")]
        public void HeaderBlockTest_3()
        {
            const string markup = "###### Hallo";

            var document = Document.Parse(markup);
            
            Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(document.ToString()));
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HeaderBlockTest_4()
        {
            const string markup = "##### Hallo # Huhu";

            var document = Document.Parse(markup);
            
            Assert.AreEqual(markup, TrimNewLinesAndWhitespaces(document.ToString()));
        }
    }
}