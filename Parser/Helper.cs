using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Parser
{
    public static class Helper
    {
       public static List<string> SplitIntoLines(this string text)
        {
            return Regex.Split(text, "\r\n|\r|\n").ToList();
        }
    }

    

}