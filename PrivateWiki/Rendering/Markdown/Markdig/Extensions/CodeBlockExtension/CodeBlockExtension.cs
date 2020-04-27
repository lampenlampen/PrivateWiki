using System;
using ColorCode;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.SyntaxHighlighting;

namespace PrivateWiki.Rendering.Markdown.Markdig.Extensions.CodeBlockExtension
{
	internal class CodeBlockExtension : IMarkdownExtension
	{
		private readonly IStyleSheet? _customCss;

		public CodeBlockExtension(IStyleSheet? customCss = null)
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

			htmlRenderer.ObjectRenderers.AddIfNotAlready(new MyHtmlCodeBlockRenderer(originalCodeBlockRenderer,
				_customCss));
		}
	}
}