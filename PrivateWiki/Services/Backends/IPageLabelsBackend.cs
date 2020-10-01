using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateWiki.Services.Backends
{
	public interface IPageLabelsBackend
	{
		Task<IEnumerable<Guid>> GetLabelIdsForPageId(Guid pageId);
	}
}