using System;
using System.Collections.Generic;

namespace Parser
{
    /// <summary>
    /// This class represents a Markdown Document.
    /// </summary>
    public class MarkdownDocument
    {
        public List<MarkdownElement> Dom { get; } = new List<MarkdownElement>();


        public static MarkdownDocument Parse(string text)
        {
            var doc = new MarkdownDocument();
            
            var a = text.Split(new [] { "\n\n", "\r\r"}, StringSplitOptions.None);

            foreach (var t in a)
            {
                var paragraph = t.TrimStart(' ');
                
                if (paragraph.StartsWith("#"))
                {
                    // Paragraph is a Heading
                    doc.Dom.Add(MarkdownHeading.Parse(paragraph));
                }
                else
                {
                    // Paragraph is a text Block
                    doc.Dom.Add(MarkdownText.Parse(paragraph));
                }
            }

            return doc;
        }
    }
}