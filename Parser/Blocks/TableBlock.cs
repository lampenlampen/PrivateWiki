using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Parser.Blocks.Table;

namespace Parser.Blocks
{
    public class TableBlock : Block
    {
        /// <summary>
        /// Gets or sets the table rows.
        /// </summary>
        public List<TableRow> Rows { get; set; }
        
        /// <summary>
        /// Gets or sets describes the columns in the table.  Rows can have more or less cells than the number
        /// of columns.  Rows with fewer cells should be padded with empty cells.  For rows with
        /// more cells, the extra cells should be hidden.
        /// </summary>
        public List<TableColumnDefinition> ColumnDefinitions { get; set; }

        internal static TableBlock Parse(List<string> lines)
        {
            // Parse the Header Row.
            var rows = new List<TableRow> {TableRow.Parse(lines[0])};

            // Parse the second row
            var columnDefinitions = new List<TableColumnDefinition>(rows[0].Cells.Count);

            var columnDefinitionsText = lines[1].Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries).Select(c => c.Trim()).ToList();

            // First and second Row must have the same amount of cells.
            if (rows[0].Cells.Count != columnDefinitionsText.Count)
            {
                throw new ArgumentException("Not a valid TableBlock!", nameof(lines));
            }

            foreach (var column in columnDefinitionsText)
            {
                if (column.Count(c => c == '-') < 3)
                {
                    throw new ArgumentException("ColumnDefinition has to contain 3 or more Dashes");
                }

                if (column.Trim().Count(c => c == '-') + column.Trim().Count(c => c == ':') == column.Length)
                {
                    throw new ArgumentException("ColumnDefinition must only contain colons (:) or dashes (-)");
                }
                
                if (column.StartsWith(":") && column.EndsWith(":"))
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Center));                    
                } else if (column.StartsWith(":"))
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Left));
                } else if (column.EndsWith(":"))
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Right));
                } else
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Unspecified));
                }
            }
            
        }
    }
}