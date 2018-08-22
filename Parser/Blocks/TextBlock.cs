using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks
{
    public class TextBlock : Block
    {
        public string Text { get; }

        private TextBlock(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        internal static TextBlock Parse(List<string> lines)
        {
            // TODO Parse Text
            // TODO Trimm two or more whitespaces.
            var textBuilder = new StringBuilder();

            for (var i = 0; i < lines.Count -1; i++)
            {
                textBuilder.AppendLine(lines[i]);
            }

            textBuilder.Append(lines[lines.Count - 1]);
            
            return new TextBlock(textBuilder.ToString());
        }

        public override string ToString()
        {
            return Text;
        }
    }
}