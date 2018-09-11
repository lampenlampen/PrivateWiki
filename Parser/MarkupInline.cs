using Parser.Enums;

namespace Parser
{
    /// <summary>
    /// An internal class that is the base class for inline elements.
    /// </summary>
    public abstract class MarkupInline
    {
        /// <summary>
        /// Gets or sets the Inline Type.
        /// </summary>
        public InlineType Type { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MarkupInline"/> class.
        /// </summary>
        /// <param name="type"></param>
        internal MarkupInline(InlineType type)
        {
            Type = type;
        }
    }
}