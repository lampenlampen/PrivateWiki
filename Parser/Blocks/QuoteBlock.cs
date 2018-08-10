using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parser.Blocks
{
    public class QuoteBlock : Container
    {
        public QuoteBlock(IEnumerable<Block> blocks) : base(blocks)
        {
           
        }

        internal static QuoteBlock Parse(List<string> lines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith(">"))
                {
                    throw new ArgumentException("Markdown Text is not a Blockquote!", nameof(lines));
                }

                lines[i]= lines[i].Substring(2);
            }

            var lastLine = lines.Last();
            if (!lastLine.StartsWith(">"))
            {
                throw new ArgumentException("Markdown Text is not a Blockquote!", nameof(lines));
            }

            return new QuoteBlock(Document.ParseLines(lines));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            
            foreach (var block in Blocks)
            {
                stringBuilder.AppendLine(block.ToString());
                stringBuilder.AppendLine("");
            }

            return stringBuilder.ToString();
        }
    }
}