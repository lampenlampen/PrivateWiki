using System;
using System.Collections.Generic;
using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using Markdig;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdigHeadingBlock = Markdig.Syntax.HeadingBlock;

namespace DataAccessLibrary.Markdig
{
	public class Parser
	{
		public static MarkdownDocument ParseToMarkdownDocument(string markdown)
		{
			return Markdown.Parse(markdown);
		}

		public static Document ParseMarkdown(string markdown)
		{
			var blocks = new List<IPageBlock>();
			var markdigDoc = ParseToMarkdownDocument(markdown);

			foreach (var block in markdigDoc)
			{
				switch (block)
				{
					case ParagraphBlock paragraphBlock:
						var textBlock = new MarkdownTextBlock(markdown.Substring(paragraphBlock.Span.Start, paragraphBlock.Span.Length), paragraphBlock);
						blocks.Add(textBlock);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(block));
				}
			}

			return new Document(blocks);
		}

		public static ContainerInline ParseMarkdownText(string markdown)
		{
			var markdigDoc = ParseToMarkdownDocument(markdown);
			var block = markdigDoc[0];

			if (markdigDoc.Count == 1 && block is ParagraphBlock paragraphBlock)
			{
				return paragraphBlock.Inline;
			}
			
			throw new Exception("Text contains more than one blocks.");
		}
	}
}