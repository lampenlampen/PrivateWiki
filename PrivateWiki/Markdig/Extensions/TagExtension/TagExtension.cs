using Markdig;
using Markdig.Renderers;

namespace PrivateWiki.Markdig.Extensions.TagExtension
{
	public class TagExtension : IMarkdownExtension
	{
		public void Setup(MarkdownPipelineBuilder pipeline)
		{
			if (!pipeline.InlineParsers.Contains<TagParser>())
			{
				pipeline.InlineParsers.AddIfNotAlready(new TagParser());
			}
		}

		public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
		{
			
		}
	}
}