using Markdig;
using Markdig.Extensions.Mathematics;
using Markdig.Renderers;

namespace PrivateWiki.Parser.Markdig
{
    class MyMathematicsExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            // Adds the inline parser
            if (!pipeline.InlineParsers.Contains<MathInlineParser>())
            {
                pipeline.InlineParsers.Insert(0, new MathInlineParser());
            }

            // Adds the block parser
            if (!pipeline.BlockParsers.Contains<MathBlockParser>())
            {
                pipeline.BlockParsers.Insert(0, new MathBlockParser());
            }
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            if (renderer is HtmlRenderer)
            {
                if (!renderer.ObjectRenderers.Contains<MyHtmlMathInlineRenderer>())
                {
                    renderer.ObjectRenderers.Insert(0, new MyHtmlMathInlineRenderer());
                }

                if (!renderer.ObjectRenderers.Contains<HtmlMathBlockRenderer>())
                {
                    renderer.ObjectRenderers.Insert(0, new HtmlMathBlockRenderer());
                }
            }
        }
    }
}
