using System.Collections.Generic;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.MostRecentlyVisitedPageService
{
	public class MostRecentlyViewedPagesManager : IMostRecentlyVisitedPagesService
	{
		private readonly List<MostRecentlyViewedPagesItem> _mostRecentlyViewedPages;

		private readonly int _limit;

		public MostRecentlyViewedPagesManager()
		{
			_limit = 4;

			_mostRecentlyViewedPages = new List<MostRecentlyViewedPagesItem>();
		}

		public void AddPage(GenericPage page)
		{
			for (var i = 0; i < _mostRecentlyViewedPages.Count; i++)
			{
				var item = _mostRecentlyViewedPages[i];

				if (item.Id.Equals(page.Id))
				{
					_mostRecentlyViewedPages.RemoveAt(i);
					break;
				}
			}

			_mostRecentlyViewedPages.Add(new MostRecentlyViewedPagesItem(page.Id, page.Path));

			Normalize();
		}

		private void Normalize()
		{
			var excessiveItemsCount = _mostRecentlyViewedPages.Count - _limit;

			if (excessiveItemsCount > 0)
			{
				_mostRecentlyViewedPages.RemoveRange(0, excessiveItemsCount);
			}
		}

		public IEnumerable<MostRecentlyViewedPagesItem> ToList()
		{
			return _mostRecentlyViewedPages.AsReadOnly();
		}
	}
}