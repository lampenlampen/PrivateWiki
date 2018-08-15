using System;
using System.Text;
using Parser.Enums;

namespace Parser.Blocks
{
    /// <summary>
    /// Represents a heading.
    /// </summary>
    public class HeaderBlock : Block
    {
        public readonly string HeadingText;
        public readonly int Level;

        private HeaderBlock(string headingText, int level)
        {
            HeadingText = headingText;
            Level = level;
        }

        public static HeaderBlock Parse(string heading)
        {
            var headingLevel = 0;

            // Count the Level of the heading
            while (heading[headingLevel].Equals('#'))
            {
                headingLevel++;
            }

            // Max supported Header Level is 5
            if (headingLevel > 5)
            {
                // Error Heading Level not supported.
                throw new ArgumentException($"Heading Level ({headingLevel}) not supported!", nameof(heading));
            }

            return new HeaderBlock(heading.Substring(headingLevel).Trim(), headingLevel);
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            for (var i = 0; i < Level; i++)
            {
                stringBuilder.Append("#");
            }

            stringBuilder.Append(" ");
            stringBuilder.Append(HeadingText);

            return stringBuilder.ToString();
        }
    }
}