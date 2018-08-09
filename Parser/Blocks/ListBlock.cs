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
            } else if (markdown.StartsWith("*"))
            {
                listblock.Style = ListStyle.Numbered;
            } else if (markdown.StartsWith("[]") || markdown.StartsWith("[x]") || markdown.StartsWith("[X]"))
            {
                listblock.Style = ListStyle.Checkboxed;
            }

            var lines = markdown.Split(new[] {"\n", "\r"}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (line.StartsWith("-"))
                {
                    
                }
            }



            return listblock;
        } 
    }
}