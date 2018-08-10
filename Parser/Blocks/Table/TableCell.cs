using System.Collections.Generic;

namespace Parser.Blocks.Table
{
    /// <summary>
    /// Represents a cell in the table.
    /// </summary>
    public class TableCell
    {
        /// <summary>
        /// Gets or sets the cell contents.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }
    }
}