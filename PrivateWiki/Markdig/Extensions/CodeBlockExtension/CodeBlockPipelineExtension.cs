using ColorCode;
using Markdig;

namespace PrivateWiki.Markdig.Extensions.CodeBlockExtension
{
	public static class CodeBlockPipelineExtension
	{
		public static MarkdownPipelineBuilder UseMyHtmlCodeBlockRenderer(this MarkdownPipelineBuilder pipeline,
			IStyleSheet customCss = null)
		{
			pipeline.Extensions.AddIfNotAlready(new CodeBlockExtension(customCss));
			return pipeline;
		}
	}
}