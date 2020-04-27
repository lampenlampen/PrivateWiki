using ColorCode;
using Markdig;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.CodeBlockExtension
{
	public static class CodeBlockPipelineExtension
	{
		public static MarkdownPipelineBuilder UseMyHtmlCodeBlockRenderer(this MarkdownPipelineBuilder pipeline,
			IStyleSheet? customCss = null)
		{
			pipeline.Extensions.AddIfNotAlready(new CodeBlockExtension(customCss));
			return pipeline;
		}
	}
}