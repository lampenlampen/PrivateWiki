using System;
using System.Collections.Generic;
using System.Text;

namespace Parser.Blocks
{
    public class TextBlock : Block
    {
        public string Text { get; }

        private TextBlock(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public static TextBlock Parse(List<string> lines)
        {
            // TODO Parse Text
            // TODO Trimm two or more whitespaces.
            var textBuilder = new StringBuilder();

            foreach (var line in lines)
            {
                var trimmedLine = line.Trim();
                textBuilder.Append(trimmedLine + " ");

                if (line.EndsWith("  "))
                {
                    textBuilder.AppendLine("");
                }
            }
            
            return new TextBlock(textBuilder.ToString());
        }
    }
}