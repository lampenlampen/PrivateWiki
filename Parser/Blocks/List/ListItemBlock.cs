using System.Collections.Generic;

namespace Parser.Blocks.List
{
    /// <summary>
    /// This specifies the Contents of the List Item.
    /// </summary>
    public class ListItemBlock
    {
        public IList<Block> Blocks { get; set; }

        internal ListItemBlock()
        {
        }
    }
}