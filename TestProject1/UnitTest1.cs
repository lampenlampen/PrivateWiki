using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            const int headingLevel = 3;
            const string headingText = "";

            var headingBuilder = new StringBuilder();

            for (var i = 0; i < headingLevel; i++)
            {
                headingBuilder.Append("#");
            }

            headingBuilder.Append(" ").Append(headingText);

            var testText = headingBuilder.ToString();

            var heading = MarkdownDocument.Parse(testText).Dom.First();
            
            System.Console.WriteLine(((MarkdownHeading) heading).HeadingText);
            System.Console.WriteLine(((MarkdownHeading) heading).Level);


        }
    }
}