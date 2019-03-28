using System;
using DataAccessLibrary.Renderer;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using MarkdigHeadingBlock = Markdig.Syntax.HeadingBlock;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class TitleBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public ContainerInline Inline { get; set; }
		
		public SourceSpan Span { get; set; }

		public TitleBlock(ContainerInline inline, SourceSpan span)
		{
			Id = Guid.NewGuid();
			Inline = inline;
			Span = span;
		}

		public string GetSourceMarkdown(string source)
		{
			return source.Substring(Span.Start, Span.Length);
		}
	}
}