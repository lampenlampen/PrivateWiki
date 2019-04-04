using System;
using System.Collections.Generic;
using System.Dynamic;
using DataAccessLibrary.PageAST.Blocks;
using JetBrains.Annotations;
using NodaTime;

namespace DataAccessLibrary.PageAST
{
	public class Document
	{
		[NotNull]
		public Guid Id { get; set; }
		
		[NotNull]
		public TitleBlock Title { get; set; }
		
		[NotNull]
		public Instant CreationTime { get; set; }
		
		[NotNull]
		public Instant ChangeTime { get; set; }
		
		[NotNull]
		public string Identifier { get; set; }
		
		public List<IPageBlock> Blocks { get; set; }

		public Document([NotNull] List<IPageBlock> blocks)
		{
			Blocks = blocks;
		}
	}
}