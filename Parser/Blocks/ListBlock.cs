using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Parser.Blocks.List;
using Parser.Enums;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks
{
    public class ListBlock : Block
    {
        public ListStyle Style { get; set; }

        public IList<ListElement> Items { get; set; }

        public ListBlock(ListStyle style, IList<ListElement> items)
        {
            Style = style;
            Items = items;
        }

        internal static ListBlock Parse(List<string> lines)
        {
            List<ListElement> listElements = new List<ListElement>();            
            
            // TODO Standard ListStyle
            var style = ListStyle.Bulleted;
            
            
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

            // Parse lines
            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                
                
                if (line.StartsWith("-"))
                {
                    // Parse Bulleted List
                    
                    var j = i + 1;
                    while (j < lines.Count && !lines[j].StartsWith("-"))
                    {
                        j++;
                    }
                    
                    // ListElement spans the lines i to (j-i).

                    listElements.Add(ListElement.Parse(lines.GetRange(i, j-i).Select(l => l.Substring(2)).ToList()));

                    i = j-1;
                } else if (line.StartsWith("*"))
                {
                    // Parse Numbered List
                    
                    var j = i + 1;
                    while (j < lines.Count && !lines[j].StartsWith("*"))
                    {
                        j++;
                    }
                    
                    // ListElement spans the lines i to (j-i).
                    
                    listElements.Add(ListElement.Parse(lines.GetRange(i, j-i).Select(l => l.Substring(2)).ToList()));

                    i = j - 1;
                } else if (line.StartsWith("["))
                {
                    // Parse Checkboxed List
                    
                    var j = i + 1;
                    while (j < lines.Count && !lines[j].StartsWith("["))
                    {
                        j++;
                    }
                    
                    // ListElements spans the lines i to (j-i).

                    // TODO Parse checked Checkbox
                    
                    listElements.Add(ListElement.Parse(lines.GetRange(i, j-i).Select(l => l.Substring(3)).ToList()));
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

                    for (var i = 0; i < Items.Count-1; i++)
                    {
                        var itemLines = Items[i].ToString().SplitIntoLines();

                        builder.AppendLine($"- {itemLines[0]}");

                        for (var j = 1; j < itemLines.Count; j++)
                        {
                            builder.AppendLine($"  {itemLines[j]}");
                        }
                    }
                    
                    // Last Line with no newline character at the end
                    var itemLinesLastLine = Items[Items.Count-1].ToString().SplitIntoLines();

                    builder.Append($"- {itemLinesLastLine[0]}");

                    if (itemLinesLastLine.Count > 1)
                    {
                        builder.AppendLine("");
                        
                        for (var j = 1; j < itemLinesLastLine.Count -1; j++)
                        {
                            builder.AppendLine($"  {itemLinesLastLine[j]}");
                        }
                        
                        builder.Append($"  {itemLinesLastLine[itemLinesLastLine.Count -1]}");    
                    }

                    break;
                case ListStyle.Checkboxed:
                    break;
                case ListStyle.Numbered:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
            return builder.ToString();
        }
    }
}