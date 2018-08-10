namespace Parser.Blocks.Table
{
    /// <summary>
    /// Describes a column in the markdown table.
    /// </summary>
    public class TableColumnDefinition
    {
        /// <summary>
        /// Gets or sets the alignment of content in a table column.
        /// </summary>
        public ColumnAlignment Alignment { get; set; }

        public TableColumnDefinition(ColumnAlignment alignment)
        {
            Alignment = alignment;
        }
    }
}