using System;
using System.Collections.Generic;
using NodaTime;

namespace PrivateWiki.DataModels.Pages
{
	public abstract record Page
	{
		[Obsolete]
		protected Page() { }

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

		protected Page(Path path, Guid id, string content, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null)
		{
			Path = path;
			Id = id;
			Content = content;
			Created = created;
			LastChanged = lastChanged;
			IsLocked = isLocked;
			Tags = tags ?? new List<Tag>();
		}

		protected Page(Path path, string content, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null)
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

		public Guid Id { get; [Obsolete] set; }

		public string Content { get; [Obsolete] set; }

		public Instant Created { get; [Obsolete] set; }

		public Instant LastChanged { get; [Obsolete] set; }

		public bool IsLocked { get; [Obsolete] set; }

		public Path Path { get; [Obsolete] internal set; }

		public List<Tag> Tags { get; }

		[Obsolete] public IList<Label> Labels { get; } = Label.GetTestData();
	}
}