using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using System;
using System.Collections.Generic;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer.Html
{
	public abstract class HtmlRenderer : IHtmlRenderer
	{
		public abstract string RenderToHtml(IPageBlock block);

		public abstract void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer);

		public static void RenderToHtml2(Document doc, MarkdigHtmlRenderer htmlRenderer)
		{
			var renderers = new List<IHtmlRenderer>
			{
				new CodeBlockRenderer(),
				new MarkdownBlockRenderer(),
				new TitleBlockRenderer()
			};

			foreach (var block in doc.Blocks)
			{
				switch (block)
				{
					case null:
						throw new ArgumentNullException(nameof(block));
					case CodeBlock codeBlock:
						var codeBlockRenderer = renderers.Find(r => r is CodeBlockRenderer);
						codeBlockRenderer.RenderToHtml(codeBlock, htmlRenderer);
						break;
					case HeadingBlock headingBlock:
						break;
					case MarkdownBlock markdownBlock:
						var markdownBlockRenderer = renderers.Find(r => r is MarkdownBlockRenderer);
						markdownBlockRenderer.RenderToHtml(markdownBlock, htmlRenderer);
						break;
					case MarkdownTextBlock textBlock:
						break;
					case TitleBlock titleBlock:
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(block));
				}
			}
		}
	}
}