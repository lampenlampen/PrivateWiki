using System;
using System.Collections.Generic;
using NodaTime;

namespace PrivateWiki.DataModels.Pages
{
	public record GenericPage : Page
	{
		public new string Link
		{
			get => Path.FullPath;
			set => Path = Path.ofLink(value);
		}

		//public override string GetContentType() => "unknown";

		public new ContentType ContentType { get; set; }

		[Obsolete]
		public GenericPage() { }

		[Obsolete]
		public GenericPage(Path path, string content, string contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, content, created, lastChanged,
			isLocked,
			tags)
		{
			ContentType = ContentType.Parse(contentType);
		}

		public GenericPage(Guid id, Path path, string content, ContentType contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, id, content, created,
			lastChanged, isLocked,
			tags)
		{
			ContentType = contentType;
		}

		public GenericPage(Path path, string content, ContentType contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, content, created, lastChanged,
			isLocked,
			tags)
		{
			ContentType = contentType;
		}

		public override string ToString() => Path.FullPath;
	}
}