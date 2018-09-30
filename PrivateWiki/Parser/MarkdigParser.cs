using Markdig;
using Markdig.SyntaxHighlighting;
using StorageProvider;

namespace PrivateWiki.Parser
{
    public class MarkdigParser
    {
        MarkdownPipeline pipeline;

        private void Init()
        {
            if (pipeline == null) pipeline = new MarkdownPipelineBuilder()
                    .UseAdvancedExtensions()
                    .UseYamlFrontMatter()
                    .UseSyntaxHighlighting()
                    .Build();
        }

        public Markdig.Syntax.MarkdownDocument Parse(string markdown)
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
