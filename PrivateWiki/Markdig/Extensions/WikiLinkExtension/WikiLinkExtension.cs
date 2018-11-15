using Markdig;
using Markdig.Extensions.AutoLinks;
using Markdig.Renderers;
using Markdig.Renderers.Normalize;
using Markdig.Renderers.Normalize.Inlines;

namespace PrivateWiki.Markdig.Extensions.WikiLinkExtension
{
    class WikiLinkExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<WikiLinkParser>())
            {
                // Insert the parser before any other parsers
                pipeline.InlineParsers.Insert(0, new WikiLinkParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            var normalizeRenderer = renderer as NormalizeRenderer;

            if(normalizeRenderer != null && !normalizeRenderer.ObjectRenderers.Contains<NormalizeAutoLinkRenderer>())
            {
                normalizeRenderer.ObjectRenderers.InsertBefore<LinkInlineRenderer>(new NormalizeAutoLinkRenderer());
            }
        }
    }
}
