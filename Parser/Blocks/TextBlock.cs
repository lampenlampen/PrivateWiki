using System;

namespace Parser.Blocks
{
    public class TextBlock : Block
    {
        public readonly string Text;

        private TextBlock(string text)
        {
            Text = text ?? throw new ArgumentNullException(nameof(text));
        }

        public static TextBlock Parse(string text)
        {
            return new TextBlock(text);
        }
    }
}