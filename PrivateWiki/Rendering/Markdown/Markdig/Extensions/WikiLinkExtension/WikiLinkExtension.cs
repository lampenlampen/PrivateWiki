﻿using Markdig;
using Markdig.Extensions.AutoLinks;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Renderers.Normalize;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.WikiLinkExtension
{
	internal class WikiLinkExtension : IMarkdownExtension
	{
		public void Setup(MarkdownPipelineBuilder pipeline)
		{
			if (!pipeline.InlineParsers.Contains<WikiLinkParser>())
				pipeline.InlineParsers.Insert(0, new WikiLinkParser());
		}

		public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
		{
			var normalizeRenderer = renderer as NormalizeRenderer;

			if (normalizeRenderer != null && !normalizeRenderer.ObjectRenderers.Contains<NormalizeAutoLinkRenderer>())
				normalizeRenderer.ObjectRenderers.InsertBefore<LinkInlineRenderer>(new NormalizeAutoLinkRenderer());
		}
	}

	public static class WikiLinkPipelineExtension
	{
		public static MarkdownPipelineBuilder UseMyWikiLinkExtension(this MarkdownPipelineBuilder pipeline)
		{
			pipeline.Extensions.AddIfNotAlready<WikiLinkExtension>();
			return pipeline;
		}
	}
}