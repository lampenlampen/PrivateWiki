using System;
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
			var document = new Document();
			var markdigDoc = ParseToMarkdownDocument(markdown);

			foreach (var block in markdigDoc)
			{
				switch (block)
				{
					case ParagraphBlock paragraphBlock:
						var textBlock = new TextBlock(markdown.Substring(paragraphBlock.Span.Start, paragraphBlock.Span.Length), paragraphBlock);
						document.Blocks.Add(textBlock);
						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(block));
				}
			}

			return document;
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