using Parser.Exceptions;

namespace Parser.Blocks.Table
{
    /// <summary>
    ///     Describes a column in the markdown table.
    /// </summary>
    public class TableColumnDefinition
	{
		public TableColumnDefinition(ColumnAlignment alignment)
		{
			Alignment = alignment;
		}

        /// <summary>
        ///     Gets or sets the alignment of content in a table column.
        /// </summary>
        public ColumnAlignment Alignment { get; set; }

		public override string ToString()
		{
			switch (Alignment)
			{
				case ColumnAlignment.Unspecified:
					return "---";
				case ColumnAlignment.Left:
					return ":---";
				case ColumnAlignment.Right:
					return "---:";
				case ColumnAlignment.Center:
					return ":---:";
				default:
					throw new TableBlockException($"Alignment ({Alignment}) does not exist.");
			}
		}
	}
}