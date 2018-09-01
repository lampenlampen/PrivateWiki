using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Parser.Blocks.Table;
using Parser.Exceptions;

[assembly: InternalsVisibleTo("TestProject1")]

namespace Parser.Blocks
{
    public class TableBlock : Block
    {
        /// <summary>
        /// Gets or sets the table rows.
        /// </summary>
        public List<TableRow> Rows { get;}

        /// <summary>
        /// Gets or sets describes the columns in the table.  Rows can have more or less cells than the number
        /// of columns.  Rows with fewer cells should be padded with empty cells.  For rows with
        /// more cells, the extra cells should be hidden.
        /// </summary>
        public List<TableColumnDefinition> ColumnDefinitions { get;}

        internal TableBlock(List<TableRow> rows, List<TableColumnDefinition> columnDefinitions)
        {
            Type = BlockType.TableBlock;
            Rows = rows;
            ColumnDefinitions = columnDefinitions;
        }

        internal static TableBlock Parse(List<string> lines, int blockStartLine = -1)
        {
            // Parse the Header Row.
            var rows = new List<TableRow> {TableRow.ParseHeader(lines[0])};

            // Parse the second row
            var columnDefinitions = new List<TableColumnDefinition>(rows[0].Cells.Count);

            var columnDefinitionsText = lines[1].Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(c => c.Trim()).ToList();

            // First and second Row must have the same amount of cells.
            if (rows[0].Cells.Count != columnDefinitionsText.Count)
            {
                var builder = new StringBuilder();
                builder.AppendLine("The number of cells of the header and the alignment row must be equal.");
                builder.AppendLine($"Number of Headers: {rows[0].Cells.Count}");
                builder.AppendLine($"Number of Alignment Options: {columnDefinitions.Count}");

                throw new TableBlockException(builder.ToString(), lines, 0, blockStartLine);
            }

            // Parse the second Row for the ColumnDefinitions
            foreach (var column in columnDefinitionsText)
            {
                if (column.Count(c => c == '-') < 3)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine("Every Alignment Option must contain 3 or more dashes (-)");
                    builder.AppendLine($"The Alignment Option Cell ({column}) contains less than 3 dashes.");
                    
                    throw new TableBlockException(builder.ToString(), lines, 1, blockStartLine);
                }

                if (column.Count(c => c == '-') + column.Count(c => c == ':') + column.Count(c => c == ' ') !=
                    column.Length)
                {
                    var builder = new StringBuilder();
                    builder.AppendLine("Every Alignment Option must only contain colons (:) or dashes (-).");
                    builder.AppendLine($"The Alignment Option Cell ({column}) contains other characters.");
                    
                    throw new TableBlockException(builder.ToString(), lines, 1, blockStartLine);
                }

                if (column.StartsWith(":") && column.EndsWith(":"))
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Center));
                }
                else if (column.StartsWith(":"))
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Left));
                }
                else if (column.EndsWith(":"))
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Right));
                }
                else
                {
                    columnDefinitions.Add(new TableColumnDefinition(ColumnAlignment.Unspecified));
                }
            }

            // Parse the actual Table Data

            for (var i = 2; i < lines.Count; i++)
            {
                rows.Add(TableRow.Parse(lines[i]));
            }

            return new TableBlock(rows, columnDefinitions);
        }


        public override string ToString()
        {
            var textBuilder = new StringBuilder();

            // Header
            textBuilder.AppendLine(Rows[0].ToString());

            // ColumnDefinitions
            textBuilder.Append("|");
            foreach (var definition in ColumnDefinitions)
            {
                textBuilder.Append($"{definition}|");
            }

            textBuilder.AppendLine("");

            // Table Data
            for (var i = 1; i < Rows.Count - 1; i++)
            {
                textBuilder.AppendLine(Rows[i].ToString());
            }

            textBuilder.Append(Rows.Last());

            return textBuilder.ToString();
        }
    }
}