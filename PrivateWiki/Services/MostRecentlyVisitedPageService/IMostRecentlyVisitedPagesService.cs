using System.Collections.Generic;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.MostRecentlyVisitedPageService
{
	public interface IMostRecentlyVisitedPagesService
	{
		void AddPage(GenericPage page);

		IEnumerable<MostRecentlyViewedPagesItem> ToList();
	}
}