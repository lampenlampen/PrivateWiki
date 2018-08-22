using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1.Blocks
{
    [TestClass]
    public class QuoteBlockUnitTest : ParserTestBase
    {
        [TestMethod]
        public void QuoteBlockTest_1()
        {
            var markup = "> hallo";

            var block = QuoteBlock.Parse(markup.SplitIntoLines());
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod]
        public void QuoteBlockTest_2()
        {
            var markup = "> hallo\r\n> Huhu\r\n> hihi\r\n> haskljb";

            var block = QuoteBlock.Parse(markup.SplitIntoLines());
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Markup Text is not a Blockquote!")]
        public void QuoteBlockTest_3()
        {
            var markup = "> hallo\r\n> Huhu\r\nhihi\r\n> haskljb";

            var block = QuoteBlock.Parse(markup.SplitIntoLines());
           
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Markup Text is not a Blockquote!")]
        public void QuoteBlockTest_4()
        {
            var markup = "> hallo\r\n> Huhu\r\n  hihi\r\n> haskljb";

            var block = QuoteBlock.Parse(markup.SplitIntoLines());
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Markup Text is not a Blockquote!")]
        public void QuoteBlockTest_5()
        {
            var markup = "> hallo\r\n> Huhu\r\n > hihi\r\n> haskljb";

            var block = QuoteBlock.Parse(markup.SplitIntoLines());
           
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod]
        public void QuoteBlockTest_6()
        {
            var markup = "> hallo\r\n> Huhu\r\n> > hihi\r\n> haskljb";

            var block = QuoteBlock.Parse(markup.SplitIntoLines());
            
            Assert.AreEqual(markup, block.ToString());
        }
    }
}