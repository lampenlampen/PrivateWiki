using System.Collections.Generic;
using System.Text;

namespace Parser
{
    public class Container: Block
    {
        public IList<Block> Blocks { get; set; }

        public Container(IList<Block> blocks)
        {
            Blocks = blocks;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (int i = 0; i < Blocks.Count -1; i++)
            {
                builder.AppendLine(Blocks[i].ToString());
            }

            builder.Append(Blocks[Blocks.Count - 1]);
            
            return builder.ToString();
        }
    }
}