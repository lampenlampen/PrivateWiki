namespace Parser
{
    /// <summary>
    ///     This class represents an Markdown Element.
    ///     A MarkdownDocument exists of several MarkdownElements.
    /// </summary>
    public abstract class Block
	{
		public BlockType Type;

		public abstract override string ToString();
	}
}