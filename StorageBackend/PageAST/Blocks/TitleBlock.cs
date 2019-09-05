using System;

namespace DataAccessLibrary.PageAST.Blocks
{
	public class TitleBlock : IPageBlock
	{

		/* Unmerged change from project 'DataAccessLibrary (netcoreapp3.0)'
		Before:
				public Guid Id { get; set; }

				public string Title { get; set; }
		After:
				public Guid Id { get; set; }

				public string Title { get; set; }
		*/
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