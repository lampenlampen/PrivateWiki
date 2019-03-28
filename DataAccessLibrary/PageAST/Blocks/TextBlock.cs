using System;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class TextBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public ParagraphBlock Content { get; set; }
		
		public ContainerInline Inlines { get; set; }
		
		public string SourceCode { get; set; }

		public TextBlock(string sourceCode, ParagraphBlock block)
		{
			Id = Guid.NewGuid();
			SourceCode = sourceCode;
			Content = block;
		}

		public TextBlock(string sourceCode, ContainerInline inlines)
		{
			Id = Guid.NewGuid();
			SourceCode = sourceCode;
			Inlines = inlines;
		}
	}
}