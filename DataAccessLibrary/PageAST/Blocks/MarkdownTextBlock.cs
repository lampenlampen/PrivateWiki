using System;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class MarkdownTextBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public ParagraphBlock Content { get; set; }
		
		public ContainerInline Inlines { get; set; }
		
		public string SourceCode { get; set; }

		public MarkdownTextBlock(string sourceCode, ParagraphBlock block)
		{
			Id = Guid.NewGuid();
			SourceCode = sourceCode;
			Content = block;
		}

		public MarkdownTextBlock(string sourceCode, ContainerInline inlines)
		{
			Id = Guid.NewGuid();
			SourceCode = sourceCode;
			Inlines = inlines;
		}
	}
}