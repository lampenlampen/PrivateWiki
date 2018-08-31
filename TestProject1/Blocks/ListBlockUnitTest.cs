using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1.Blocks
{
    [TestClass]
    public class ListBlockUnitTest
    {
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_1()
        {
            var markup = "- huhu\r\n- haha\r\n  hihi\r\n- hoho";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_2()
        {
            var markup = "- huhu\r\n- haha\r\n  hihi\r\n- hoho\r\n  hoho2";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_3()
        {
            var markup = "- huhu\r\n- haha\r\n  hihi\r\n- hoho\r\n- \r\n- hllsdf";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_4()
        {
            var markup = "* huhu\r\n* haha\r\n  hihi\r\n* hoho";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_5()
        {
            var markup = "* huhu\r\n* haha\r\n  hihi\r\n* hoho\r\n  hoho2";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_6()
        {
            var markup = "* huhu\r\n* haha\r\n  hihi\r\n* hoho\r\n* \r\n* hllsdf";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_7()
        {
            var markup = "[] huhu\r\n[] haha\r\n   hihi\r\n[] hoho";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_8()
        {
            var markup = "[] huhu\r\n[] haha\r\n   hihi\r\n[] hoho\r\n   hoho2";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListBlockTest_9()
        {
            var markup = "[] huhu\r\n[] haha\r\n   hihi\r\n[] hoho\r\n[] \r\n[] hllsdf";

            var block = ListBlock.Parse(markup.SplitIntoLines(), 0);
            
            Assert.AreEqual(markup, block.ToString());
        }
        
    }
}