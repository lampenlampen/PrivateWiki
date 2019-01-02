using System.Collections.Generic;
using Parser.Enums;

namespace Parser.Inlines
{
    /// <summary>
    ///     Represents a span containing code, or other text that is to be displayed a fixed-width font.
    /// </summary>
    public class CodeInline : MarkupInline, IInlineLeaf
	{
        /// <summary>
        ///     Initializes a new instance of <see cref="CodeInline" />.
        /// </summary>
        public CodeInline(string text) : base(InlineType.Code)
		{
			Text = text;
		}

        /// <summary>
        ///     Gets or sets the text to display as code.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///     Returns the chars that if found means we might have a match.
        /// </summary>
        internal static void AddTripChars(List<InlineTripChar> tripCharHelpers)
		{
			tripCharHelpers.Add(new InlineTripChar {FirstChar = '`', Method = InlineParseMethod.Code});
		}

		internal static CodeInline Parse(string text)
		{
			// TODO Alternate Syntax: ``Hello`` == "Hello"


			// Check the first char
			if (string.IsNullOrEmpty(text) || !text.StartsWith("`") || !text.EndsWith("`")) return null;

			var inline = new CodeInline(text);

			return inline;
		}

		public override string ToString()
		{
			return Text == null ? base.ToString() : $"`{Text}`";
		}
	}
}