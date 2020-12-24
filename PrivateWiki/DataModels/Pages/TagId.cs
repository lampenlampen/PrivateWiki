using System;

namespace PrivateWiki.DataModels.Pages
{
	public record TagId : IdBase
	{
		public TagId(Guid id) : base(id) { }
	}
}