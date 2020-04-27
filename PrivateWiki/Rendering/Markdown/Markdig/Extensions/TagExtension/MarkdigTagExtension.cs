using Markdig;
using Markdig.Syntax;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.TagExtension
{
	public static class MarkdigTagExtension
	{
		public static MarkdownPipelineBuilder UseTagExtension(this MarkdownPipelineBuilder pipeline)
		{
			pipeline.Extensions.AddIfNotAlready<Extensions.TagExtension.TagExtension>();
			return pipeline;
		}

		public static TagGroup GetTags(this MarkdownDocument doc)
		{
			return doc.GetData(TagParser.DocumentKey) as TagGroup;
		}
	}
}