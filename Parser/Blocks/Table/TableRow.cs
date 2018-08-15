using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        private TableRow(IList<TableCell> cells)
        {
            Cells = cells;
        }

        internal static TableRow Parse(string text)
        {
           var cells = new List<TableCell>();
            
            // TODO Parse TableRow
            var textCells = text.Split(new[] {"|"}, StringSplitOptions.None).Select(c => c.Trim()).ToList();

            foreach (var cell in textCells)
            {
                cells.Add(TableCell.Parse(cell));
            }
            
            return new TableRow(cells);
        }

        public override string ToString()
        {
            // TODO TableRow ToString

            var textBuilder = new StringBuilder();

            textBuilder.Append("| ");

            foreach (var cell in Cells)
            {
                textBuilder.Append($"{cell} |");
            }

            return textBuilder.ToString();
        }
    }
}