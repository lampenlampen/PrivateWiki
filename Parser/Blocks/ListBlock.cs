using System;
using System.Collections.Generic;
using Parser.Blocks.List;
using Parser.Enums;

namespace Parser.Blocks
{
    public class ListBlock : Block
    {
        public ListStyle Style { get; set; }

        public IList<ListItemBlock> Items { get; set; }

        internal static ListBlock Parse(string markdown)
        {
            var listblock = new ListBlock();

            if (markdown.StartsWith("-"))
            {
                listblock.Style = ListStyle.Bulleted;
            }
            else if (markdown.StartsWith("*"))
            {
                listblock.Style = ListStyle.Numbered;
            }
            else if (markdown.StartsWith("[]") || markdown.StartsWith("[x]") || markdown.StartsWith("[X]"))
            {
                listblock.Style = ListStyle.Checkboxed;
            }

            var lines = markdown.Split(new[] {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                if (line.StartsWith("-"))
                {
                    var ListItem = new ListItemBlock();

                    var j = i + 1;
                    while (j < lines.Length)
                    {
                        if (lines[j].StartsWith("-"))
                        {
                            
                        }
                        
                        
                        j++;
                    }
                }
            }

            return listblock;
        }

        public override string ToString()
        {
            // TODO Override ToString()
            return base.ToString();
        }
    }
}