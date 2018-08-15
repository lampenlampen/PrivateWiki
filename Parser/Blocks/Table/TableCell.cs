using System.Collections.Generic;
using System.Net.Mime;

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

        public string Text;

        public TableCell(string text)
        {
            Text = text;
        }

        public TableCell(IList<MarkdownInline> inlines)
        {
            Inlines = inlines;
        }

        public static TableCell Parse(string cell)
        {
            // TODO TableCell Parse
            return new TableCell(cell);
            
            
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            // TODO TableCell ToString
            return Text;
        }
    }
}