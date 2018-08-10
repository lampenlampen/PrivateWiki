using System.Collections.Generic;
using System.Linq;
using Parser.Blocks;

namespace Parser
{
    /// <summary>
    /// This class represents a Markdown Document.
    /// </summary>
    public class Document
    {
        public IList<Block> Dom { get; }

        public Document(IList<Block> blocks)
        {
            Dom = blocks;
        }

        public static Document Parse(string text)
        {
            var blocks = ParseLines(text.SplitIntoLines());

            return new Document(blocks);
        }

        public static IList<Block> ParseLines(List<string> lines)
        {
            List<Block> blocks = new List<Block>();

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];

                if (line.StartsWith("#"))
                {
                    // Line is a Heading
                    blocks.Add(HeaderBlock.Parse(line));
                    continue;
                }
                else if (line.StartsWith(">"))
                {
                    // Line is QuoteBlock

                    var j = i + 1;
                    while (lines[j][0] == '>')
                    {
                        j++;
                    }
                    
                    // Line j is not part of the Quoteblock anymore.
                    // The Quoteblock spans the lines i to (j-i).
                    
                    blocks.Add(QuoteBlock.Parse(lines.GetRange(i, j-i)));

                    // Skip lines i to (j-i), because they belong to the Quoteblock.
                    i = j - i;
                    continue;
                    
                } else if (line.StartsWith("---"))
                {
                    // Line is a Horizontal Line.
                    blocks.Add(HorizontalLine.Parse(line));
                    continue;
                } else if (line.StartsWith("```"))
                {
                    // Line begins a Codeblock.
                    
                    // Determine CodeLanguage
                    string codeLanguage = null;

                    var codeString = line.Substring(3).Trim();
                    if (!codeString.Equals(""))
                    {
                        codeLanguage = codeString;
                    }

                    // Find End of Codeblock
                    var j = i + 1;
                    while (!lines[j].StartsWith("```"))
                    {
                        j++;
                    }
                    
                    // Line j is not part of the Codeblock anymore.
                    // The Codeblock spans the lines i to (j-i).
                    
                    blocks.Add(CodeBlock.Parse(lines.GetRange(i, j-i), codeLanguage));
                    
                    // Skip lines i to (j-i), because they belong to the Codeblock.
                    i = j - 1;
                    continue;
                } else if (line.StartsWith("|"))
                {
                    // Line is a TableBlock.
                    // TODO Parse Table

                    var j = i + 1;
                    while (!lines[j].StartsWith("|"))
                    {
                        j++;
                    }
                    
                    // Line j is not part of the Tableblock anymore.
                    // The Tableblock spans the lines i to (j-i).
                    
                    blocks.Add(TableBlock.Parse(lines.GetRange(i, j-i)));
                }
                else
                {
                    // Line is a Textblock

                    var j = i + 1;
                    while (!lines[j].Trim().Equals(""))
                    {
                        j++;
                    }
                    
                    // Line j is a Empty line.
                    // The Textblock spans the lines i to (j-i).
                    
                    blocks.Add(TextBlock.Parse(lines.GetRange(i, j-i)));
                    
                    // Skip lines i to (j-i), because they belong to the Textblock.
                }
            }

            return blocks;
        }
    }
}