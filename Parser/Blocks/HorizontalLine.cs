using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks
{
    public class HorizontalLine : Block
    {
        public HorizontalLine()
        {
        }

        internal static HorizontalLine Parse(string text)
        {
            if (text.StartsWith("---") && text.Count(c => c == '-') == text.Trim().Length)
            {
                return new HorizontalLine();
            }
            throw new ArgumentException("Text represents not a Horizontal Line!", nameof(text));
        }

        public override string ToString()
        {
            return "---";
        }
    }
}