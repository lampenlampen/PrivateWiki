using System;

namespace PrivateWiki.DataModels.Pages
{
	public record PageId : IdBase
	{
		public PageId(Guid id) : base(id) { }
	}
}