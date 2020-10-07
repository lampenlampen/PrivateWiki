using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PrivateWiki.Services.Backends
{
	public interface IPageLabelsBackend
	{
		Task<IEnumerable<Guid>> GetLabelIdsForPageId(Guid pageId);

		Task AddLabelToPage(Guid pageId, Guid labelId);

		Task DeleteLabel(Guid labelId);

		Task DeletePage(Guid pageId);

		Task RemoveLabelFromPage(Guid pageId, Guid labelId);
	}
}