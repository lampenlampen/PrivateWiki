using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1
{
    [TestClass]
    public class HeaderBlockUnitTest
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
    }
}