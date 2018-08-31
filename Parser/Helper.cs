using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;

namespace Parser
{
    public static class Helper
    {
       public static List<string> SplitIntoLines(this string text)
        {
            return Regex.Split(text, "\r\n|\r|\n").ToList();
        }

        public static string JoinToString(this List<string> list)
        {
            var builder = new StringBuilder();

            for (int i = 0; i < list.Count -1; i++)
            {
                builder.AppendLine(list[i]);
            }

            builder.Append(list[list.Count - 1]);

            return builder.ToString();
        }
    }

    

}