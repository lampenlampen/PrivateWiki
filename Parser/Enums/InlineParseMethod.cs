namespace Parser.Enums
{
	public enum InlineParseMethod
	{
        /// <summary>
        ///     A Comment text
        /// </summary>
        Comment,

        /// <summary>
        ///     A Link Reference
        /// </summary>
        LinkReference,

        /// <summary>
        ///     A bold element
        /// </summary>
        Bold,

        /// <summary>
        ///     An bold and italic block
        /// </summary>
        BoldItalic,

        /// <summary>
        ///     A code element
        /// </summary>
        Code,

        /// <summary>
        ///     An italic block
        /// </summary>
        Italic,

        /// <summary>
        ///     A link block
        /// </summary>
        MarkdownLink,

        /// <summary>
        ///     An angle bracket link.
        /// </summary>
        AngleBracketLink,

        /// <summary>
        ///     A url block
        /// </summary>
        Url,

        /// <summary>
        ///     A reddit style link
        /// </summary>
        RedditLink,

        /// <summary>
        ///     An in line text link
        /// </summary>
        PartialLink,

        /// <summary>
        ///     An email element
        /// </summary>
        Email,

        /// <summary>
        ///     strike through element
        /// </summary>
        Strikethrough,

        /// <summary>
        ///     Super script element.
        /// </summary>
        Superscript,

        /// <summary>
        ///     Image element.
        /// </summary>
        Image,

        /// <summary>
        ///     Emoji element.
        /// </summary>
        Emoji
	}
}