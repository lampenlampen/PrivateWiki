using Markdig.Syntax;
using System;

namespace StorageBackend.PageAST.Blocks
{
	public class MarkdownBlock : IPageBlock
	{
		public Guid Id { get; set; }

		public MarkdownDocument Content { get; set; }

		public string Source { get; set; }

		public MarkdownBlock(MarkdownDocument doc, string source)
		{
			Id = Guid.NewGuid();
			Source = source;
			Content = doc;
		}

		public MarkdownBlock(Guid id, MarkdownDocument content, string source)
		{
			Id = id;
			Content = content;
			Source = source;
		}
	}
}