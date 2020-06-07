using System;
using System.Collections.Generic;
using NodaTime;

namespace PrivateWiki.DataModels.Pages
{
	public class GenericPage : Page
	{
		public new string Link
		{
			get => Path.FullPath;
			set => Path = Path.ofLink(value);
		}

		//public override string GetContentType() => "unknown";

		public new ContentType ContentType { get; set; }

		[Obsolete]
		public GenericPage()
		{
		}

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

		public GenericPage Clone(string? content = null)
		{
			if (content == null)
			{
				return new GenericPage(Id, Path, Content, ContentType, Created, LastChanged, IsLocked, Tags);
			}
			else
			{
				return new GenericPage(Id, Path, content, ContentType, Created, LastChanged, IsLocked, Tags);
			}
		}
	}
}