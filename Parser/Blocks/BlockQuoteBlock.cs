using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Parser.Blocks
{
    public class BlockQuoteBlock : Block
    {
        public IList<Block> Blocks { get; set; }
        public String QuoteDisplayName { get; set; }
        public String QuoteLink { get; set; }


        internal BlockQuoteBlock Parse(string markdown)
        {
            var lines = markdown.Split(new[] {"\n", "\r"}, StringSplitOptions.None);

            for (var i = 0; i < lines.Length-1; i++)
            {
                var line = lines[i];
                if (!line.StartsWith(">"))
                {
                    throw new ArgumentException("Markdown Text is not a Blockquote!", nameof(markdown));
                }

                lines[i] = line.Substring(2);
            }

            var lastLine = lines.Last();
            if (!lastLine.StartsWith(">"))
            {
                throw new ArgumentException("Markdown Test is not a Blockquote!", nameof(markdown));
            }

            if (lastLine.StartsWith(">>"))
            {
                var beginDisplayName = 0;
                var endDisplayName = 0;
                var beginLink = 0;
                var endLink = 0;

                foreach (var c in lastLine)
                {
                    if (c.Equals('['))
                    {
                        
                    }
                }
            }

        }
    }
}