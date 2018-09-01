namespace Parser.Blocks
{
    public class MathBlock : Block
    {
        private string Text { get; }

        public MathBlock(string text)
        {
            Type = BlockType.MathBlock;
            Text = text;
        }

        internal static MathBlock Parse(string line, int mathBlockStartLine)
        {
           return new MathBlock(line);
        }

        public override string ToString()
        {
            return $"\\[{Text}\\]";
        }
    }
}