﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1
{
    [TestClass]
    public class HorizontalLineUnitTest : ParserTestBase
    {
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalLine_Dashes_Correct()
        {
            const string markup = "---";
            var block = new HorizontalLine();
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalLine_Dashes_Correct_2()
        {
            const string markup = "---";
            var block = new HorizontalLine();

            var document = Document.Parse(markup);
           
            Assert.AreEqual(TrimNewLinesAndWhitespaces(document.ToString()), block.ToString());
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalLine_Dashes_Correct_3()
        {
            const string markup = "---  ";
            var block = new HorizontalLine();

            var document = Document.Parse(markup);
           
            Assert.AreEqual(TrimNewLinesAndWhitespaces(document.ToString()), block.ToString());
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        public void HorizontalLine_Dashes_Correct_4()
        {
            const string markup = "---  ";
            var block = new HorizontalLine();

            var document = Document.Parse(markup);
           
            Assert.AreEqual(TrimNewLinesAndWhitespaces(document.ToString()), block.ToString());
        }
    }
}