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

		public bool IsDeleted;

		public HistoryMarkdownPage()
		{
		}

		public HistoryMarkdownPage(Guid id, string link, string content, Instant created, Instant lastChanged, bool isLocked, Instant validFrom, Instant validTo, bool isDeleted) : base(id, link, content, created, lastChanged, isLocked)
		{
			ValidFrom = validFrom;
			ValidTo = validTo;
			IsDeleted = isDeleted;
		}
	}

	public class DeletedHistoryMarkdownPage : HistoryMarkdownPage
	{

	}

	public class CreatedHistoryMarkdownPage : HistoryMarkdownPage
	{

	}

	public class LockedHistoryMarkdownPage : HistoryMarkdownPage
	{

	}

	public class UnlockedHistoryMarkdownPage : HistoryMarkdownPage
	{

	}

	public class EditedHistoryMarkdownPage : HistoryMarkdownPage
	{

	}
}
