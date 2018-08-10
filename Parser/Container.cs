using System.Collections.Generic;

namespace Parser
{
    public class Container: Block
    {
        public IEnumerable<Block> Blocks { get; set; }

        public Container(IEnumerable<Block> blocks)
        {
            Blocks = blocks;
        }
    }
}