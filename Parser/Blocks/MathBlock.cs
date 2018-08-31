namespace Parser.Blocks
{
    public class MathBlock : Block
    {
        private string text;

        public MathBlock(string text)
        {
            this.text = text;
        }

        internal static MathBlock Parse(string line, int mathBlockStartLine)
        {
           return new MathBlock(line);
        }

        public override string ToString()
        {
            return $"\\[{text}\\]";
        }
    }
}