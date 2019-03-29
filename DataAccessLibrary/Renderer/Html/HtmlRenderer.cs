using System;
using System.Collections.Generic;
using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer.Html
{
	public abstract class HtmlRenderer : IHtmlRenderer
	{
		public MarkdigHtmlRenderer _renderer;

		public abstract string RenderToHtml(IPageBlock block);

		public abstract void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer);

		public static void RenderToHtml(IEnumerable<IPageBlock> blocks, MarkdigHtmlRenderer renderer)
		{
			foreach (var block in blocks)
			{
				switch (block)
				{
					case null:
						throw new ArgumentNullException(nameof(block));
					case CodeBlock codeBlock:
							
						break;
					case HeadingBlock headingBlock:
						break;
					case MarkdownBlock markdownBlock:
						break;
					case TextBlock textBlock:
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