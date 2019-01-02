using System.Collections.Generic;
using System.Text;
using Parser.Blocks;

namespace Parser
{
    /// <summary>
    ///     This class represents a Markdown Document.
    /// </summary>
    public class Document
	{
		public Document(IList<Block> blocks)
		{
			Dom = blocks;
		}

		public IList<Block> Dom { get; }

		public static Document Parse(string text)
		{
			var blocks = ParseLines(text.SplitIntoLines());

			return new Document(blocks);
		}

		public static IList<Block> ParseLines(List<string> lines)
		{
			var blocks = new List<Block>();

			for (var i = 0; i < lines.Count; i++)
			{
				var line = lines[i];

				if (string.IsNullOrEmpty(line))
				{
				}
				else if (line.StartsWith("#"))
				{
					// Line is a Heading
					blocks.Add(HeaderBlock.Parse(line));
				}
				else if (line.StartsWith(">"))
				{
					// Line is QuoteBlock

					var j = i + 1;
					while (j < lines.Count && lines[j].StartsWith("> ")) j++;

					// Line j is not part of the Quoteblock anymore.
					// The Quoteblock spans the lines i to (j-i).

					blocks.Add(QuoteBlock.Parse(lines.GetRange(i, j - i)));

					// Skip lines i to (j-i), because they belong to the Quoteblock.
					i = j - i;
				}
				else if (line.StartsWith("---"))
				{
					// Line is a Horizontal Line.
					blocks.Add(HorizontalRuleBlock.Parse(line));
				}
				else if (line.StartsWith("```"))
				{
					// Line begins a Codeblock.

					// Determine CodeLanguage
					string codeLanguage = null;

					var codeString = line.Substring(3).Trim();
					if (!codeString.Equals("")) codeLanguage = codeString;

					// Find End of Codeblock
					var j = i + 1;
					while (j < lines.Count && !lines[j].StartsWith("```")) j++;

					// Line j is not part of the Codeblock anymore.
					// The Codeblock spans the lines i to (j-i).

					blocks.Add(CodeBlock.Parse(lines.GetRange(i + 1, j - i - 1), codeLanguage));

					// Skip lines i to (j-i), because they belong to the Codeblock.
					i = j;
				}
				else if (line.StartsWith("|"))
				{
					// Line is a TableBlock.

					var j = i + 1;
					while (j < lines.Count && lines[j].StartsWith("|")) j++;

					// Line j is not part of the Tableblock anymore.
					// The Tableblock spans the lines i to (j-i).

					blocks.Add(TableBlock.Parse(lines.GetRange(i, j - i)));

					// Skip lines i to (j-i), because they belong to the Codeblock.
					i = j - 1;
				}
				else if (line.StartsWith("[") || line.StartsWith("*") || line.StartsWith("-"))
				{
					// Line is a ListBlock.

					var j = i + 1;
					while (j < lines.Count && (line.StartsWith("[") || line.StartsWith("*") || line.StartsWith("-") ||
					                           line.StartsWith("\n") || line.StartsWith("\r")))
						j++;

					// The ListBlock spans the lines i to (j-i).

					blocks.Add(ListBlock.Parse(lines.GetRange(i, j - i), i));

					// Skip lines i to (j-i), because they belong to the Codeblock.
					i = j - 1;
				}
				else if (line.StartsWith("\\[") && line.EndsWith("\\]"))
				{
					// Line is a MathBlock.
					blocks.Add(MathBlock.Parse(line.Substring(2, line.Length - 3), i));
				}
				else
				{
					// Line is a Textblock

					var j = i + 1;
					while (j < lines.Count && !lines[j].Trim().Equals("")) j++;

					// Line j is a Empty line.
					// The Textblock spans the lines i to (j-i).

					blocks.Add(TextBlock.Parse(lines.GetRange(i, j - i)));

					// Skip lines i to (j-i), because they belong to the TextBlock.
					i = j - 1;
				}
			}

			return blocks;
		}

		public override string ToString()
		{
			var textBuilder = new StringBuilder();

			for (var i = 0; i < Dom.Count - 1; i++) textBuilder.AppendLine(Dom[i].ToString());
			textBuilder.Append(Dom[Dom.Count - 1]);

			return textBuilder.ToString();
		}
	}
}