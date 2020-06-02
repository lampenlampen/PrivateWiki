using Markdig;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.MathExtension
{
	internal static class MathePipelineExtension
	{
		public static MarkdownPipelineBuilder UseMyMathExtension(this MarkdownPipelineBuilder pipeline)
		{
			pipeline.Extensions.AddIfNotAlready<MyMathematicsExtension>();
			return pipeline;
		}
	}
}