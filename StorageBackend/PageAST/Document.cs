using JetBrains.Annotations;
using NodaTime;
using System;
using System.Collections.Generic;
using StorageBackend.PageAST.Blocks;

namespace StorageBackend.PageAST
{
	public class Document
	{
		[NotNull]
		public Guid Id { get; set; }

		public TitleBlock Title { get; set; }

		[NotNull]
		public Instant CreationTime { get; set; }

		[NotNull]
		public Instant ChangeTime { get; set; }

		[NotNull]
		public string Link { get; set; }

		public List<IPageBlock> Blocks { get; set; }

		public string Content { get; set; }

		public Document([NotNull] List<IPageBlock> blocks)
		{
			Blocks = blocks;
		}

		public Document(Guid id, TitleBlock title, Instant creationTime, Instant changeTime, [NotNull] string link)
		{
			Id = id;
			Title = title;
			CreationTime = creationTime;
			ChangeTime = changeTime;
			Link = link;
		}

		public Document(TitleBlock title, Instant creationTime, Instant changeTime, [NotNull] string link, List<IPageBlock> blocks)
		{
			Id = Guid.NewGuid();
			Title = title;
			CreationTime = creationTime;
			ChangeTime = changeTime;
			Link = link;
			Blocks = blocks;
		}

		public Document(Guid id, TitleBlock title, Instant creationTime, Instant changeTime, [NotNull] string link, string content)
		{
			Id = id;
			Title = title;
			CreationTime = creationTime;
			ChangeTime = changeTime;
			Link = link;
			Content = content;
		}


	}
}