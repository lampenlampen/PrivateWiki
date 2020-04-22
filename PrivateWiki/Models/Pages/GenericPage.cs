using System;
using System.Collections.Generic;
using NodaTime;
using PrivateWiki.Data;

namespace PrivateWiki.Models.Pages
{
	public class GenericPage : Page
	{
		public new string ContentType
		{
			get => ContentType2.Name;
			set => ContentType2 = Data.ContentType.Parse(value);
		}

		public new string Link
		{
			get => Path.FullPath;
			set => Path = Path.ofLink(value);
		}

		public override string GetContentType() => "unknown";

		public ContentType ContentType2 { get; private set; }

		[Obsolete]
		public GenericPage()
		{
		}

		[Obsolete]
		public GenericPage(Path path, string content, string contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, content, created, lastChanged,
			isLocked,
			tags)
		{
			ContentType = contentType;
			ContentType2 = Data.ContentType.Parse(contentType);
		}

		public GenericPage(Guid id, Path path, string content, ContentType contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, id, content, created,
			lastChanged, isLocked,
			tags)
		{
			ContentType = contentType.ToString();
			ContentType2 = contentType;
		}

		public GenericPage(Path path, string content, ContentType contentType, Instant created, Instant lastChanged, bool isLocked, List<Tag>? tags = null) : base(path, content, created, lastChanged,
			isLocked,
			tags)
		{
			ContentType = contentType.ToString();
			ContentType2 = contentType;
		}

		public GenericPage Clone(string? content = null)
		{
			if (content == null)
			{
				return new GenericPage(Id, Path, Content, ContentType2, Created, LastChanged, IsLocked, Tags);
			}
			else
			{
				return new GenericPage(Id, Path, content, ContentType2, Created, LastChanged, IsLocked, Tags);
			}
		}
	}
}