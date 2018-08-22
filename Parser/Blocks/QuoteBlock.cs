using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks
{
    public class QuoteBlock : Container
    {
        public QuoteBlock(IList<Block> blocks) : base(blocks)
        {
           
        }

        internal static QuoteBlock Parse(List<string> lines)
        {
            for (var i = 0; i < lines.Count; i++)
            {
                if (!lines[i].StartsWith(">"))
                {
                    throw new ArgumentException($"Markdown Text is not a Blockquote!\n", nameof(lines));
                }

                lines[i]= lines[i].Substring(2);
            }

            return new QuoteBlock(Document.ParseLines(lines));
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var lines = base.ToString().SplitIntoLines();

            for (var i = 0; i < lines.Count - 1; i++)
            {
                stringBuilder.AppendLine(lines[i].Insert(0, "> "));
            }
            stringBuilder.Append(lines[lines.Count -1].Insert(0, "> "));

            return stringBuilder.ToString();
        }
    }
}