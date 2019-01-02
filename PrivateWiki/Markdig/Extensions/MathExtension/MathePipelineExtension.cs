using Markdig;

namespace PrivateWiki.Markdig.Extensions.MathExtension
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