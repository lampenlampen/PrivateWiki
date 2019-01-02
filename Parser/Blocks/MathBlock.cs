namespace Parser.Blocks
{
	public class MathBlock : Block
	{
		public MathBlock(string text)
		{
			Type = BlockType.MathBlock;
			Text = text;
		}

		private string Text { get; }

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