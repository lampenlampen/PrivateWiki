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
        public void ListrBlockTest_1()
        {
            var markup = "- huhu\r\n- haha\r\n  hihi\r\n- hoho";

            var block = ListBlock.Parse(markup.SplitIntoLines());
            
            Console.WriteLine($"ListElements: {block.Items.Count}");
            Console.WriteLine($"0: {block.Items[0]}");
            Console.WriteLine($"1: {block.Items[1]}");
            Console.WriteLine($"2: {block.Items[2]}");
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListrBlockTest_2()
        {
            var markup = "- huhu\r\n- haha\r\n  hihi\r\n- hoho\r\n  hoho2";

            var block = ListBlock.Parse(markup.SplitIntoLines());
            
            Console.WriteLine($"ListElements: {block.Items.Count}");
            Console.WriteLine($"0: {block.Items[0]}");
            Console.WriteLine($"1: {block.Items[1]}");
            Console.WriteLine($"2: {block.Items[2]}");
            
            Assert.AreEqual(markup, block.ToString());
        }
        
        [TestMethod] [TestCategory("Parse - block")]
        public void ListrBlockTest_3()
        {
            var markup = "- huhu\r\n- haha\r\n  hihi\r\n- hoho\r\nhllsdf";

            var block = ListBlock.Parse(markup.SplitIntoLines());
            
            Console.WriteLine($"ListElements: {block.Items.Count}");
            Console.WriteLine($"0: {block.Items[0]}");
            Console.WriteLine($"1: {block.Items[1]}");
            Console.WriteLine($"2: {block.Items[2]}");
            
            Assert.AreEqual(markup, block.ToString());
        }
        
    }
}