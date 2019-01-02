using Markdig;

namespace PrivateWiki.Markdig.Extensions.MathExtension
{
	static class MathePipelineExtension
	{
		public static MarkdownPipelineBuilder UseMyMathExtension(this MarkdownPipelineBuilder pipeline)
		{
			pipeline.Extensions.AddIfNotAlready<MyMathematicsExtension>();
			return pipeline;
		}
	}
}