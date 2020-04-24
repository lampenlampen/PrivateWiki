using System;
using System.Collections.Generic;
using NodaTime;

namespace PrivateWiki.Models.Pages
{
	public abstract class Page
	{
		protected Page()
		{
		}

		[Obsolete]
		protected Page(string link, Guid id, string content, Instant created, Instant lastChanged, bool isLocked)
		{
			//Link = link;
			Id = id;
			Content = content;
			Created = created;
			LastChanged = lastChanged;
			IsLocked = isLocked;
			Path = Path.ofLink(link);
		}

		protected Page(Path path, Guid id, string content, Instant created, Instant lastChanged, bool isLocked, List<Tag> tags = null)
		{
			Path = path;
			Id = id;
			Content = content;
			Created = created;
			LastChanged = lastChanged;
			IsLocked = isLocked;
			Tags = tags ?? new List<Tag>();
		}

		protected Page(Path path, string content, Instant created, Instant lastChanged, bool isLocked, List<Tag> tags = null)
		{
			Path = path;
			Id = Guid.NewGuid();
			Content = content;
			Created = created;
			LastChanged = lastChanged;
			IsLocked = isLocked;

			Tags = tags ?? new List<Tag>();
		}

		[Obsolete] public string Link => Path.FullPath;

		public Guid Id { get; set; }

		public string Content { get; set; }

		public Instant Created { get; set; }

		public Instant LastChanged { get; set; }

		public bool IsLocked { get; set; }

		public Path Path { get; set; }

		public List<Tag> Tags { get; }

		//public string ContentType => GetContentType();

		//public abstract string GetContentType();
	}
}