using System;
using System.Collections.Generic;
using NodaTime;

namespace PrivateWiki.Models.Pages
{
	public class GenericPage : Page
	{
		public new string ContentType;

		public new string Link
		{
			get => Path.FullPath;
			set => Path = Path.ofLink(value);
		}

		public override string GetContentType() => "unknown";

		[Obsolete]
		public GenericPage(string link, Guid id, string content, string contentType, Instant created, Instant lastChanged, bool isLocked) : base(link, id, content, created, lastChanged, isLocked)
		{
			ContentType = contentType;
		}

		[Obsolete]
		public GenericPage()
		{
		}

		public GenericPage(Guid id, Path path, string content, string contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, id, content, created,
			lastChanged, isLocked,
			tags)
		{
			ContentType = contentType;
		}

		public GenericPage(Path path, string content, string contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, content, created, lastChanged,
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