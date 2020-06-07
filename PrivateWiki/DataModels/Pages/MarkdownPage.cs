using System;
using NodaTime;

namespace PrivateWiki.DataModels.Pages
{
	public class MarkdownPage : Page
	{
		public MarkdownPage()
		{
		}

		public MarkdownPage(Guid id, string link, string content, Instant created, Instant lastChanged, bool isLocked) : base(link, id, content, created, lastChanged, isLocked)
		{
		}

		public string GetContentType() => "markdown";

		public string ContentType => GetContentType();
	}
}