using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonMark;
using CommonMark.Syntax;

namespace PrivateWiki
{
    internal class CustomHtmlFormatter : CommonMark.Formatters.HtmlFormatter
    {
        public CustomHtmlFormatter(System.IO.TextWriter target, CommonMarkSettings settings)
        : base(target, settings)
        {
        }


        protected override void WriteBlock(Block block, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            switch (block.Tag)
            {
                case BlockTag.FencedCode:
                case BlockTag.IndentedCode:

                    ignoreChildNodes = true;

                    EnsureNewLine();
                    Write("<pre><code");
                    if (Settings.TrackSourcePosition) WritePositionAttribute(block);

                    var info = block.FencedCodeData?.Info;
                    if (!string.IsNullOrEmpty(info))
                    {
                        var x = info.IndexOf(' ');
                        if (x == -1)
                            x = info.Length;

                        Write(" class=\"language-");
                        //WriteEncodedHtml(new StringPart(info, 0, x));
                        Write('\"');
                    }
                    Write('>');
                    WriteEncodedHtml(block.StringContent);
                    WriteLine("</code></pre>");
                    break;



                    break;
                default:
                    base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
                    break;
            }

            base.WriteBlock(block, isOpening, isClosing, out ignoreChildNodes);
        }

        protected override void WriteInline(Inline inline, bool isOpening, bool isClosing, out bool ignoreChildNodes)
        {
            base.WriteInline(inline, isOpening, isClosing, out ignoreChildNodes);
        }
    }
}
