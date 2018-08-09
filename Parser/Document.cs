using System;
using System.Collections.Generic;
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
            var blocks = ParseBlocks(text);

            return new Document(blocks);
        }

        public static IList<Block> ParseBlocks(string text)
        {
            IList<Block> Blocks = new List<Block>();
            
            var a = text.Split(new [] { "\n\n", "\r\r"}, StringSplitOptions.None);

            foreach (var t in a)
            {
                var paragraph = t.TrimStart(' ');
                
                if (paragraph.StartsWith("#"))
                {
                    // Paragraph is a Heading
                    Blocks.Add(HeaderBlock.Parse(paragraph));
                    
                }
                else
                {
                    // Paragraph is a text Block
                    Blocks.Add(TextBlock.Parse(paragraph));
                }
            }

            return Blocks;
        }
    }
}