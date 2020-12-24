using System;

namespace PrivateWiki.DataModels.Pages
{
	public record LabelId : IdBase
	{
		public LabelId(Guid id) : base(id) { }
	}
}