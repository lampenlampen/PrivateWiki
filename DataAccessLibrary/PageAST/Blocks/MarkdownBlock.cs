using System;
using Markdig.Syntax;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class MarkdownBlock : IPageBlock
	{
		public Guid Id { get; set; }

		public MarkdownDocument Content { get; set; }

		public SourceSpan Span { get; set; }

		public MarkdownBlock(MarkdownDocument doc, SourceSpan span)
		{
			Id = Guid.NewGuid();
			Span = span;
			Content = doc;
		}
	}
}