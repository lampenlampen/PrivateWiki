using System;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.MostRecentlyVisitedPageService
{
	public class MostRecentlyViewedPagesItem
	{
		public Guid Id { get; }

		public Path Path { get; }

		public MostRecentlyViewedPagesItem(Guid id, Path path)
		{
			Id = id;
			Path = path;
		}
	}
}