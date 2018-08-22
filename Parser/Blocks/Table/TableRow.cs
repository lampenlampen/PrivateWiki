using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("TestProject1")]
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

        internal static TableRow ParseHeader(string text)
        {
            var cells = new List<TableCell>();
            
            // TODO Parse TableRow
            var textCells = text.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList();

            foreach (var cell in textCells)
            {
                cells.Add(TableCell.Parse(cell));
            }
            
            return new TableRow(cells);
        }

        internal static TableRow Parse(string text)
        {
           var cells = new List<TableCell>();
            
            // TODO Parse TableRow
            var textCells = text.Split(new[] {"|"}, StringSplitOptions.None).ToList().Select(c => c.Trim()).ToList();

            for (var index = 1; index < textCells.Count - 1; index++)
            {
                var cell = textCells[index];
                cells.Add(TableCell.Parse(cell));
            }

            return new TableRow(cells);
        }

        public override string ToString()
        {
            // TODO TableRow ToString

            var textBuilder = new StringBuilder();

            foreach (var cell in Cells)
            {
                textBuilder.Append($"| {cell} ");
            }

            textBuilder.Append("|");

            return textBuilder.ToString();
        }
    }
}