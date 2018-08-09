using System;
using Parser.Enums;

namespace Parser.Blocks
{
    /// <summary>
    /// Represents a heading.
    /// </summary>
    public class HeaderBlock : Block
    {
        public readonly string HeadingText;
        public readonly HeadingLevel Level;

        private HeaderBlock(string headingText, HeadingLevel level)
        {
            HeadingText = headingText;
            Level = level;
        }

        public static HeaderBlock Parse(string heading)
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

            return new HeaderBlock(headingText,
                (HeadingLevel) Enum.Parse(typeof(HeadingLevel), headingLevel.ToString()));
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}