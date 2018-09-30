using Markdig;
using Markdig.Syntax;
using Markdig.SyntaxHighlighting;
using PrivateWiki.Markdig;
using StorageProvider;

namespace PrivateWiki.Parser
{
    public class MarkdigParser
    {
        MarkdownPipeline pipeline;

        private void Init()
        {
            if (pipeline == null)
            {
                var pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseYamlFrontMatter()
                    .UseSyntaxHighlighting();
                pipeline.Extensions.Add(new WikiLinkExtension());

                this.pipeline = pipeline.Build();
            }
        }

        public MarkdownDocument Parse(string markdown)
        {
            Init();

            var dom = Markdown.Parse(markdown);
            return dom;
        }

        public string ToHtmlString(string markdown)
        {
            return Markdown.ToHtml(markdown, pipeline);
        }

        public string ToHtmlString(ContentPage page)
        {
            return ToHtmlString(page.Content);
        }
    }
}
