using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks
{
    public class CodeBlock: Block
    {
        public string Text { get; set; }
        
        public string CodeLanguage { get; set; }

        public CodeBlock(string text, string codeLanguage)
        {
            Type = BlockType.CodeBlock;
            Text = text;
            CodeLanguage = codeLanguage;
        }

        internal static CodeBlock Parse(List<string> lines, string codeLanguage)
        {
            var textBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                textBuilder.AppendLine(line);
            }
            
            return new CodeBlock(textBuilder.ToString(), codeLanguage);
        }

        public override string ToString()
        {
            var textBuilder = new StringBuilder();
            
            if (CodeLanguage != null)
            {
                textBuilder.AppendLine($"``` {CodeLanguage}");
            }

            textBuilder.Append(Text);

            textBuilder.Append("```");

            return textBuilder.ToString();
        }
    }
}