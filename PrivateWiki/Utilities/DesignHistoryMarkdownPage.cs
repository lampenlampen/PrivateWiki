using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Pages;
using NodaTime;

namespace PrivateWiki.Utilities
{
	class DesignHistoryMarkdownPage
	{
		public ObservableCollection<HistoryMarkdownPage> Pages = new ObservableCollection<HistoryMarkdownPage>();

		public DesignHistoryMarkdownPage()
		{
			var page = new HistoryMarkdownPage
			{
				Id = Guid.NewGuid(),
				Link = "TestLink",
				Content = "# Hallo",
				Created = SystemClock.Instance.GetCurrentInstant(),
				LastChanged = SystemClock.Instance.GetCurrentInstant(),
				IsLocked = false,
				ValidFrom = SystemClock.Instance.GetCurrentInstant(),
				ValidTo = SystemClock.Instance.GetCurrentInstant()
		};
			Pages.Add(page);
		}
	}
}
