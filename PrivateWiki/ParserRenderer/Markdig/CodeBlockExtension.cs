using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorCode;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.SyntaxHighlighting;

namespace PrivateWiki.ParserRenderer.Markdig
{
    class CodeBlockExtension : IMarkdownExtension
    {
        private readonly IStyleSheet _customCss;

        public CodeBlockExtension(IStyleSheet customCss = null)
        {
            _customCss = customCss;
        }

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer == null) throw new ArgumentNullException(nameof(renderer));

            var htmlRenderer = renderer as TextRendererBase<HtmlRenderer>;
            if (htmlRenderer == null) return;

            var originalCodeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();
            if (originalCodeBlockRenderer != null) htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer);

            var originalCodeBlockRenderer2 =
                htmlRenderer.ObjectRenderers.FindExact<SyntaxHighlightingCodeBlockRenderer>();
            if (originalCodeBlockRenderer2 != null) htmlRenderer.ObjectRenderers.Remove(originalCodeBlockRenderer2);

            htmlRenderer.ObjectRenderers.AddIfNotAlready(new MyHtmlCodeBlockRenderer(originalCodeBlockRenderer, _customCss));
        }
    }

    public static class CodeBlockExtensions
    {
        public static MarkdownPipelineBuilder UseMyHtmlCodeBlockRenderer(this MarkdownPipelineBuilder pipeline,
            IStyleSheet customCss = null)
        {
            pipeline.Extensions.AddIfNotAlready(new CodeBlockExtension(customCss));
            return pipeline;
        }
    }

    
}