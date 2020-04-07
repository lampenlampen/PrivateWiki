using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using PrivateWiki.Data;

namespace PrivateWiki
{
	public static class AppConfig
	{
		public static readonly IList<string> SupportedContentTypes = Enum.GetNames(typeof(ContentType));

		public static readonly IList<ContentType2> SupportedContentTypes2 = new List<ContentType2> {ContentType2.Html, ContentType2.Markdown, ContentType2.Text};
	}
}