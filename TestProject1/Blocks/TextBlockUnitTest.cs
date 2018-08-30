using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1.Blocks
{
    [TestClass]
    public class TextBlockUnitTest
    {
        [TestMethod]
        [TestCategory("Parse - block")]
        public void TextBlockTest_1()
        {
            var markup = "Hallo\r\nHuhu\r\nHihi";

            var actual = TextBlock.Parse(markup.SplitIntoLines());
            
            Assert.AreEqual(markup, actual.ToString());
        }
        
        [TestMethod]
        [TestCategory("Parse - block")]
        public void TextBlockTest_2()
        {
            var markup = "Hallo\r\nHuhu\r\nHihi👍💖";

            var actual = TextBlock.Parse(markup.SplitIntoLines());
            
            Assert.AreEqual(markup, actual.ToString());
        }
        
    }
}