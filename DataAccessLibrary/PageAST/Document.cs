using System.Collections.Generic;
using System.Dynamic;

namespace DataAccessLibrary.PageAST
{
	public class Document
	{
		public List<IPageBlock> Blocks { get; set; } = new List<IPageBlock>();
	}
}