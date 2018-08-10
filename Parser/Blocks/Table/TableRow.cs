using System.Collections.Generic;

namespace Parser.Blocks.Table
{
    /// <summary>
    /// Represents a single row in the table.
    /// </summary>
    public class TableRow
    {
        /// <summary>
        /// Gets or sets the table cells.
        /// </summary>
        public IList<TableCell> Cells { get; set; }

        internal static TableRow Parse(string text)
        {
            // TODO Parse TableRow
        }
    }
}