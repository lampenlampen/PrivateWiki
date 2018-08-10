using System.Collections.Generic;
using System.Text;

namespace Parser.Blocks
{
    public class CodeBlock: Block
    {
        public string Text { get; set; }
        
        public string CodeLanguage { get; set; }

        public CodeBlock(string text, string codeLanguage)
        {
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
            if (CodeLanguage != null)
            {
                return $"``` {CodeLanguage}\n{Text}\n```";
            }

            return $"```\n{Text}\n```";
        }
    }
}