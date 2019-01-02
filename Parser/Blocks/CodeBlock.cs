using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("TestProject1")]

namespace Parser.Blocks
{
	public class CodeBlock : Block
	{
		public CodeBlock(string text, string codeLanguage)
		{
			Type = BlockType.CodeBlock;
			Text = text;
			CodeLanguage = codeLanguage;
		}

		public string Text { get; set; }

		public string CodeLanguage { get; set; }

		internal static CodeBlock Parse(List<string> lines, string codeLanguage)
		{
			var textBuilder = new StringBuilder();

			for (var i = 0; i < lines.Count - 1; i++)
			{
				var line = lines[i];
				textBuilder.AppendLine(lines[i]);
			}

			textBuilder.Append(lines[lines.Count - 1]);


			return new CodeBlock(textBuilder.ToString(), codeLanguage);
		}

		public override string ToString()
		{
			var textBuilder = new StringBuilder();

			if (CodeLanguage != null) textBuilder.AppendLine($"``` {CodeLanguage}");

			textBuilder.Append(Text);

			textBuilder.Append("```");

			return textBuilder.ToString();
		}
	}
}