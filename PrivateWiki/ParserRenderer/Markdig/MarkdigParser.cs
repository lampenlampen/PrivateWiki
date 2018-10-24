using System.Text;
using JetBrains.Annotations;
using Markdig;
using Markdig.Syntax;
using Markdig.SyntaxHighlighting;
using PrivateWiki.Parser.Markdig;
using StorageProvider;

namespace PrivateWiki.ParserRenderer.Markdig
{
    public class MarkdigParser : IPageParser, IPageRenderer
    {
        private MarkdownPipeline _pipeline;

        private void Init()
        {
            if (_pipeline == null)
            {
                var builder = new MarkdownPipelineBuilder()
                    .UseAutoIdentifiers()
                    .UseAutoLinks()
                    .UseDiagrams()
                    //.UseAdvancedExtensions()
                    .UseSyntaxHighlighting();

                builder.Extensions.AddIfNotAlready<MyMathematicsExtension>();

                _pipeline = builder.Build();

                    
                /*
                pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseSyntaxHighlighting()
                    .Build();
                */
            }
        }

        public MarkdownDocument Parse(string markdown)
        {
            var dom = Markdown.Parse(markdown, _pipeline);
            return dom;
        }

        public MarkdownDocument Parse(ContentPage page)
        {
            return Parse(page.Content);
        }

        public string ToHtmlString(string markdown)
        {
            Init();

            var builder = new StringBuilder();
            builder.AppendLine("<!DOCTYPE html>");
            builder.AppendLine("<html>");
            builder.AppendLine("<head>");
            builder.AppendLine("<script type=\"text/javascript\">");
            builder.AppendLine("MathJax.Hub.Config({tex2jax: {inlineMath: [['$', '$'], [\"\\(\", \"\\)\"]], processEscapes: true}});");
            builder.AppendLine("</script>");
            builder.AppendLine(
                "<script type=\"text/javascript\" async src=\"https://cdnjs.cloudflare.com/ajax/libs/mathjax/2.7.5/MathJax.js?config=TeX-MML-AM_CHTML\">");
            builder.AppendLine("MathJax.Hub.Register.StartupHook(\"onLoad\",function () {");
            builder.AppendLine("MathJax.Hub.Config({elements: document.querySelectorAll(\".math\")});");
            builder.AppendLine("});");
            builder.AppendLine("</script>");
            builder.AppendLine("</head>");
            builder.AppendLine("<body>");
            builder.AppendLine(Markdown.ToHtml(markdown, _pipeline));
            builder.AppendLine("</body>");
            builder.AppendLine("</html>");
            return builder.ToString();
        }

        public string ToHtmlString(ContentPage page)
        {
            return ToHtmlString(page.Content);
        }
    }
}
