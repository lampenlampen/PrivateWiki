using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

            for (var i = 0; i < lines.Count; i++)
            {
                var line = lines[i];
                
                // Parse Bulleted List
                if (line.StartsWith("-"))
                {
                    var j = i + 1;
                    while (j < lines.Count && !lines[j].StartsWith("-"))
                    {
                        j++;
                    }
                    
                    // ListElement spans the lines i to (j-i).

                    listElements.Add(ListElement.Parse(lines.GetRange(i, j-i).Select(l => l.Substring(2)).ToList()));

                    i = j;
                } else if (line.StartsWith("*"))
                {
                    var j = i + 1;
                    while (j < lines.Count && !lines[j].StartsWith("*"))
                    {
                        j++;
                    }
                    
                    // ListElement spans the lines i to (j-i).
                    
                    listElements.Add(ListElement.Parse(lines.GetRange(i, j-i).Select(l => l.Substring(2)).ToList()));
                } else if (line.StartsWith("["))
                {
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
            return base.ToString();
        }
    }
}