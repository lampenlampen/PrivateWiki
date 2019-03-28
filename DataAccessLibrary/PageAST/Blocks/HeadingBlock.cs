using System;
using MarkdigHeadingBlock = Markdig.Syntax.HeadingBlock;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class HeadingBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public MarkdigHeadingBlock Block { get; set; }
		
		public string SourceText { get; set; }

		public HeadingBlock(string sourceText, MarkdigHeadingBlock block)
		{
			Id = Guid.NewGuid();
			SourceText = sourceText;
			Block = block;
		}
	}
}