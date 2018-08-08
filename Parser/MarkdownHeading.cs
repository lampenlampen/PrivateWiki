using System;

namespace Parser
{
    public class MarkdownHeading : MarkdownElement
    {
        public readonly string HeadingText;
        public readonly HeadingLevel Level;

        private MarkdownHeading(string headingText, HeadingLevel level)
        {
            HeadingText = headingText;
            Level = level;
        }

        public static MarkdownHeading Parse(string heading)
        {
            var headingLevel = 0;
            var trimmedText = heading.TrimStart(new char[' ']);

            
            var headingText = trimmedText;
            while (headingText.StartsWith("#"))
            {
                headingLevel++;
                headingText = headingText.Substring(1);
            }

            if (headingLevel > 5)
            {
                // Error Heading Level not supported.
            }

            return new MarkdownHeading(headingText,
                (HeadingLevel) Enum.Parse(typeof(HeadingLevel), headingLevel.ToString()));
        }
    }

    public enum HeadingLevel
    {
        Heading1 = 1,
        Heading2 = 2,
        Heading3 = 3,
        Heading4 = 4,
        Heading5 = 5
    }
}