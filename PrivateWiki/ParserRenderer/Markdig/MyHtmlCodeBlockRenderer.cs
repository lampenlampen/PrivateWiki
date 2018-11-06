using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorCode;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Markdig.SyntaxHighlighting;

namespace PrivateWiki.ParserRenderer.Markdig
{
    class MyHtmlCodeBlockRenderer : SyntaxHighlightingCodeBlockRenderer
    {
        private readonly CodeBlockRenderer _underlyingRenderer;
        private readonly IStyleSheet _customCss;

        public MyHtmlCodeBlockRenderer(CodeBlockRenderer underlyingRenderer = null, IStyleSheet customCss = null)
        {
            _underlyingRenderer = underlyingRenderer ?? new CodeBlockRenderer();
            _customCss = customCss;
        }

        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            var fencedCodeBlock = obj as FencedCodeBlock;
            var parser = obj.Parser as FencedCodeBlockParser;
            if (fencedCodeBlock == null || parser == null)
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            var attributes = obj.TryGetAttributes() ?? new HtmlAttributes();

            var languageMoniker = fencedCodeBlock.Info.Replace(parser.InfoPrefix, string.Empty);
            if (string.IsNullOrEmpty(languageMoniker))
            {
                _underlyingRenderer.Write(renderer, obj);
                return;
            }

            attributes.AddClass($"lang-{languageMoniker}");
            attributes.Classes.Remove($"language-{languageMoniker}");

            attributes.AddClass("editor-colors");

            renderer
                .Write($"<div")
                .WriteAttributes(attributes)
                .Write(">");

            string firstLine;
            var code = GetCode(obj, out firstLine);

            var codeHeader = $"<div class=\"codeHeader\" style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockHeaderStyle()}\">\r\n" +
                             $"<span class=\"language\" style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockLanguageSpanStyle()}\">{languageMoniker}</span>\r\n" +
                             $"<button id=\"codeCopy\" onClick=\"codeCopyClickFunction\" class=\"action\" style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockButtonStyle()}\">\r\n" +
                             $"<span style=\"{MarkdigStyleSheet.CodeBlockHeader.GetCodeBlockButtonIconStyle()}\"> </span>\r\n" +
                             "<span>Copy</span>\r\n" +
                             "</button>\r\n" +
                             "</div>";
            renderer.Write(codeHeader);

            
            
            

            var markup = ApplySyntaxHighlighting(languageMoniker, firstLine, code);

            var markup2 = markup.Substring(56, markup.Length - 56 - 12);
            var markup3 = $"<pre style=\"{MarkdigStyleSheet.CodeBlock.GetCodeBlockBoxStyle()}\">" +
                          $"<code style=\"{MarkdigStyleSheet.CodeBlock.GetCodeBlockStyle()}\">" +
                          $"{markup2}" +
                          $"</code>" +
                          $"</pre>";

            renderer.WriteLine(markup3);
            renderer.WriteLine("</div>");
        }

        private string ApplySyntaxHighlighting(string languageMoniker, string firstLine, string code)
        {
            var languageTypeAdapter = new LanguageTypeAdapter();
            var language = languageTypeAdapter.Parse(languageMoniker, firstLine);

            if (language == null)
            { //handle unrecognised language formats, e.g. when using mermaid diagrams
                return code;
            }

            var codeBuilder = new StringBuilder();
            var codeWriter = new StringWriter(codeBuilder);
            var styleSheet = _customCss ?? StyleSheets.Default;
            var colourizer = new CodeColorizer();
            colourizer.Colorize(code, language, Formatters.Default, styleSheet, codeWriter);
            return codeBuilder.ToString();
        }

        private static string GetCode(LeafBlock obj, out string firstLine)
        {
            var code = new StringBuilder();
            firstLine = null;
            foreach (var line in obj.Lines.Lines)
            {
                var slice = line.Slice;
                if (slice.Text == null)
                {
                    continue;
                }

                var lineText = slice.Text.Substring(slice.Start, slice.Length);

                if (firstLine == null)
                {
                    firstLine = lineText;
                }
                else
                {
                    code.AppendLine();
                }

                code.Append(lineText);
            }
            return code.ToString();
        }
    }
}
