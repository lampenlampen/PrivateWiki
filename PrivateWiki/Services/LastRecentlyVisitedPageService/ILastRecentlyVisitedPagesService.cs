using System.Collections.Generic;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.LastRecentlyVisitedPageService
{
	public interface IMostRecentlyVisitedPagesService
	{
		void AddPage(GenericPage page);

		IEnumerable<MostRecentlyViewedPagesItem> ToList();
	}
}