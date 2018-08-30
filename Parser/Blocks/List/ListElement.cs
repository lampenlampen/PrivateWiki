using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly:InternalsVisibleTo("TestProject1")]
namespace Parser.Blocks.List
{
    public class ListElement
    {
        private string text;

        private ListElement(string text)
        {
            this.text = text;
        }

        internal static ListElement Parse(List<string> lines)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < lines.Count -1; i++)
            {
                var line = lines[i];
                builder.AppendLine(line);
            }

            builder.Append(lines[lines.Count - 1]);

            return new ListElement(builder.ToString());
        }

        public override string ToString()
        {
            return text;
        }
    }
}