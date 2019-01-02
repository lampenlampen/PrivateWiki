using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Parser.Blocks.List;
using Parser.Enums;
using Parser.Exceptions;

[assembly: InternalsVisibleTo("TestProject1")]

namespace Parser.Blocks
{
	public class ListBlock : Block
	{
		public ListBlock(ListStyle style, IList<ListElement> items)
		{
			Type = BlockType.ListBlock;
			Style = style;
			Items = items;
		}

		public ListStyle Style { get; set; }

		public IList<ListElement> Items { get; set; }

		internal static ListBlock Parse(List<string> lines, int blockStartLine)
		{
			var listElements = new List<ListElement>();

			// TODO Standard ListStyle
			ListStyle style;


			// Determine ListStyle

			if (lines[0].StartsWith("-"))
			{
				style = ListStyle.Bulleted;
			}
			else if (lines[0].StartsWith("*"))
			{
				style = ListStyle.Numbered;
			}
			else if (lines[0].StartsWith("[]") || lines[0].StartsWith("[x]") || lines[0].StartsWith("[X]"))
			{
				style = ListStyle.Checkboxed;
			}
			else
			{
				var builder = new StringBuilder();
				builder.AppendLine("Unkown ListStyle");
				throw new ListBlockException(builder.ToString(), lines, 0, blockStartLine);
			}

			// Parse lines
			for (var i = 0; i < lines.Count; i++)
			{
				var line = lines[i];


				if (line.StartsWith("-"))
				{
					// Parse Bulleted List

					var j = i + 1;
					while (j < lines.Count && !lines[j].StartsWith("-")) j++;

					// ListElement spans the lines i to (j-i).

					listElements.Add(ListElement.Parse(lines.GetRange(i, j - i).Select(l => l.Substring(2)).ToList()));

					i = j - 1;
				}
				else if (line.StartsWith("*"))
				{
					// Parse Numbered List

					var j = i + 1;
					while (j < lines.Count && !lines[j].StartsWith("*")) j++;

					// ListElement spans the lines i to (j-i).

					listElements.Add(ListElement.Parse(lines.GetRange(i, j - i).Select(l => l.Substring(2)).ToList()));

					i = j - 1;
				}
				else if (line.StartsWith("["))
				{
					// Parse Checkboxed List

					var j = i + 1;
					while (j < lines.Count && !lines[j].StartsWith("[")) j++;

					// ListElements spans the lines i to (j-i).

					// TODO Parse checked Checkbox

					listElements.Add(ListElement.Parse(lines.GetRange(i, j - i).Select(l => l.Substring(3)).ToList()));
				}
			}

			return new ListBlock(style, listElements);
		}

		public override string ToString()
		{
			// TODO Override ToString()
			var builder = new StringBuilder();
			switch (Style)
			{
				case ListStyle.Bulleted:

					for (var i = 0; i < Items.Count - 1; i++)
					{
						var itemLines = Items[i].ToString().SplitIntoLines();

						builder.AppendLine($"- {itemLines[0]}");

						for (var j = 1; j < itemLines.Count; j++) builder.AppendLine($"  {itemLines[j]}");
					}

					// Last Line with no newline character at the end
					var itemLinesLastLine1 = Items[Items.Count - 1].ToString().SplitIntoLines();

					builder.Append($"- {itemLinesLastLine1[0]}");

					if (itemLinesLastLine1.Count > 1)
					{
						builder.AppendLine("");

						for (var j = 1; j < itemLinesLastLine1.Count - 1; j++)
							builder.AppendLine($"  {itemLinesLastLine1[j]}");

						builder.Append($"  {itemLinesLastLine1[itemLinesLastLine1.Count - 1]}");
					}

					break;
				case ListStyle.Numbered:

					for (var i = 0; i < Items.Count - 1; i++)
					{
						var itemLines = Items[i].ToString().SplitIntoLines();

						builder.AppendLine($"* {itemLines[0]}");

						for (var j = 1; j < itemLines.Count; j++) builder.AppendLine($"  {itemLines[j]}");
					}

					// Last Line with no newline character at the end
					var itemLinesLastLine2 = Items[Items.Count - 1].ToString().SplitIntoLines();

					builder.Append($"* {itemLinesLastLine2[0]}");

					if (itemLinesLastLine2.Count > 1)
					{
						builder.AppendLine("");

						for (var j = 1; j < itemLinesLastLine2.Count - 1; j++)
							builder.AppendLine($"  {itemLinesLastLine2[j]}");

						builder.Append($"  {itemLinesLastLine2[itemLinesLastLine2.Count - 1]}");
					}

					break;
				case ListStyle.Checkboxed:

					for (var i = 0; i < Items.Count - 1; i++)
					{
						var itemLines = Items[i].ToString().SplitIntoLines();

						builder.AppendLine($"[] {itemLines[0]}");

						for (var j = 1; j < itemLines.Count; j++) builder.AppendLine($"   {itemLines[j]}");
					}

					// Last Line with no newline character at the end
					var itemLinesLastLine3 = Items[Items.Count - 1].ToString().SplitIntoLines();

					builder.Append($"[] {itemLinesLastLine3[0]}");

					if (itemLinesLastLine3.Count > 1)
					{
						builder.AppendLine("");

						for (var j = 1; j < itemLinesLastLine3.Count - 1; j++)
							builder.AppendLine($"   {itemLinesLastLine3[j]}");

						builder.Append($"   {itemLinesLastLine3[itemLinesLastLine3.Count - 1]}");
					}

					break;
				default:
					throw new ArgumentOutOfRangeException();
			}


			return builder.ToString();
		}
	}
}