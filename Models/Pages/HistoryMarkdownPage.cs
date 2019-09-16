using System;
using System.Collections.Generic;
using System.Text;
using NodaTime;

namespace Models.Pages
{
	public class HistoryMarkdownPage : MarkdownPage
	{
		public Instant ValidFrom;

		public Instant ValidTo;

		public HistoryMarkdownPage()
		{
		}

		public HistoryMarkdownPage(Guid id, string link, string content, Instant created, Instant lastChanged, bool isLocked, Instant validFrom, Instant validTo) : base(id, link, content, created, lastChanged, isLocked)
		{
			ValidFrom = validFrom;
			ValidTo = validTo;
		}
	}
}
