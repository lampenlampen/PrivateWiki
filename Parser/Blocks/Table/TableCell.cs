using System.Collections.Generic;
using System.Net.Mime;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("TestProject1")]
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

        public string Text { get; set; }

        public TableCell(string text)
        {
            Text = text;
        }

        public TableCell(IList<MarkdownInline> inlines)
        {
            Inlines = inlines;
        }

        internal static TableCell Parse(string text)
        {
            // TODO TableCell Parse
            return new TableCell(text);
        }

        public override string ToString()
        {
            // TODO TableCell ToString
            return Text;
        }
    }
}