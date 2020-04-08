using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using PrivateWiki.Data;

namespace PrivateWiki
{
	public static class AppConfig
	{
		public static readonly IList<ContentType> SupportedContentTypes2 = new List<ContentType> {ContentType.Html, ContentType.Markdown, ContentType.Text};
	}
}