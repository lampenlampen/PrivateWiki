using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMarkSharp.Blocks;

namespace PrivateWiki
{
    class CustomHtmlRenderer : CommonMarkSharp.HtmlRenderer
    {
        public override void Visit(Header header)
        {
            

            var tag = string.Format("h{0}", header.Level);

            WriteStartTag(header, tag);
            Write(header.Inlines);
            WriteEndTag(header, tag);
            WriteLine();
        }
    }
}
