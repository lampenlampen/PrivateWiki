using System;
using System.Linq;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks
{
    public class HorizontalRuleBlock : Block
    {
        public HorizontalRuleBlock()
        {
            Type = BlockType.HorizontalRuleBlock;
        }

        internal static HorizontalRuleBlock Parse(string text)
        {
            if (text.StartsWith("---") && text.Count(c => c == '-') == text.Trim().Length)
            {
                return new HorizontalRuleBlock();
            }
            throw new ArgumentException("Text represents not a Horizontal Rule!", nameof(text));
        }

        public override string ToString()
        {
            return "---";
        }
    }
}