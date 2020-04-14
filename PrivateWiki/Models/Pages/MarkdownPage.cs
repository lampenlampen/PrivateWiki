using System;
using NodaTime;

namespace PrivateWiki.Models.Pages
{
	public class MarkdownPage : Page
	{
		public MarkdownPage()
		{
		}

		public MarkdownPage(Guid id, string link, string content, Instant created, Instant lastChanged, bool isLocked) : base(link, id, content, created, lastChanged, isLocked)
		{
		}

		public override string GetContentType() => "markdown";
	}
}