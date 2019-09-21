using System;
using NodaTime;

namespace Models.Pages
{
	public abstract class Page
	{
		protected Page()
		{
		}

		protected Page(string link, Guid id, string content, Instant created, Instant lastChanged, bool isLocked)
		{
			Link = link;
			Id = id;
			Content = content;
			Created = created;
			LastChanged = lastChanged;
			IsLocked = isLocked;
		}

		public string Link { get; set; }

		public Guid Id { get; set; }

		public string Content { get; set; }

		public Instant Created { get; set; }

		public Instant LastChanged { get; set; }

		public bool IsLocked { get; set; }
	}
}