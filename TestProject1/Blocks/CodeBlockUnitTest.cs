using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parser;
using Parser.Blocks;

namespace TestProject1.Blocks
{
    [TestClass]
    public class CodeBlockUnitTest : ParserTestBase
    {
        [TestMethod]
        public void CodeBlockTest_1()
        {
            var markup = "public static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld!\");\r\n}";
            var codeLanguage = "java";
            var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

            var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CodeBlockTest_2()
        {
            var markup = "public static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld!\");\r\n}";
            var builder = new StringBuilder();
            builder.AppendLine("public class Main");
            builder.AppendLine("{");
            builder.AppendLine("    public static void main(String[] args)");
            builder.AppendLine("    {");
            builder.AppendLine("        System.out.println();");
            builder.AppendLine("    }");
            builder.AppendLine("}");

            var codeLanguage = "markdown";
            var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

            var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CodeBlockTest_3()
        {
            var markup = "public static void main(String[] args)\r\n{\r\n    System.out.println(\"HelloWorld!\");\r\n}";
            var codeLanguage = "";
            var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

            var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CodeBlockTest_4()
        {
            var markup = "";
            var codeLanguage = "java";
            var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

            var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void CodeBlockTest_5()
        {
            var markup = "";
            var codeLanguage = "";
            var expected = $"``` {codeLanguage}\r\n{markup}\r\n```";

            var actual = CodeBlock.Parse(markup.SplitIntoLines(), codeLanguage).ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}