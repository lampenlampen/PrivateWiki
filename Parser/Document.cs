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
        public List<Block> Dom { get; } = new List<Block>();


        public static Document Parse(string text)
        {
            var doc = new Document();
            
            var a = text.Split(new [] { "\n\n", "\r\r"}, StringSplitOptions.None);

            foreach (var t in a)
            {
                var paragraph = t.TrimStart(' ');
                
                if (paragraph.StartsWith("#"))
                {
                    // Paragraph is a Heading
                    doc.Dom.Add(HeaderBlock.Parse(paragraph));
                }
                else
                {
                    // Paragraph is a text Block
                    doc.Dom.Add(TextBlock.Parse(paragraph));
                }
            }

            return doc;
        }
    }
}