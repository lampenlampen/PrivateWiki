using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Blocks
{
    public class QuoteBlock : Block
    {
        public IList<Block> Blocks { get; set; }
        public String Citation { get; set; }

        public QuoteBlock(IList<Block> blocks)
        {
            Blocks = blocks;
        }


        internal QuoteBlock Parse(string markdown)
        {
            var lines = markdown.Split(new[] {"\n", "\r"}, StringSplitOptions.None);

            var quottedTextBuilder = new StringBuilder();

            
            for (var i = 0; i < lines.Length-1; i++)
            {
                var line = lines[i];
                if (!line.StartsWith(">"))
                {
                    throw new ArgumentException("Markdown Text is not a Blockquote!", nameof(markdown));
                }

                lines[i] = line.Substring(2);
                quottedTextBuilder.AppendLine(line.Substring(2));
            }

            var lastLine = lines.Last();
            if (!lastLine.StartsWith(">"))
            {
                throw new ArgumentException("Markdown Text is not a Blockquote!", nameof(markdown));
            } else if (lastLine.StartsWith(">>"))

            {
                Citation = lastLine.Substring(2).Trim();
            } else
            {
                quottedTextBuilder.AppendLine(lastLine.Substring(2));
            }

            var quottedText = quottedTextBuilder.ToString();
            
            // TODO Parse quottedText


            return new QuoteBlock(Document.ParseBlocks(quottedText));
        }
    }
}