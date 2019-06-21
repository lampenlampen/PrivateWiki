using System;
using MarkdigHeadingBlock = Markdig.Syntax.HeadingBlock;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class TitleBlock : IPageBlock
	{
		public Guid Id { get; set; }
		
		public string Title { get; set; }
		
		public string Source { get; set; }

		public TitleBlock(string title, string source)
		{
			Id = Guid.NewGuid();
			Title = title;
			Source = source;
		}

		public TitleBlock(Guid id, string title, string source)
		{
			Id = id;
			Title = title;
			Source = source;
		}
	}
}