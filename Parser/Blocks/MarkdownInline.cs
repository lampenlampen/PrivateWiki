namespace Parser.Blocks
{
    /// <summary>
    /// An internal class that is the base class for inline elements.
    /// </summary>
    public abstract class MarkdownInline
    {
        
        public InlineType Type { get; set; }

        internal MarkdownInline(InlineType type)
        {
            Type = type;
        }
    }
}