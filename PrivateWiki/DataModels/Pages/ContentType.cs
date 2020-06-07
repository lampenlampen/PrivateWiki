using System.Collections.Generic;
using System.Linq;

namespace PrivateWiki.DataModels.Pages
{
	public class ContentType
	{
		public static readonly ContentType Markdown = new ContentType("Markdown", ".md", "text/markdown");
		public static readonly ContentType Html = new ContentType("Html", ".html", "text/html");
		public static readonly ContentType Text = new ContentType("Text", ".txt", "text/plain");

		public static readonly IList<ContentType> SupportedContentTypes2 = new List<ContentType> {Html, Markdown, Text};


		public ContentType(string name, string fileExtension, string mimeType)
		{
			Name = name;
			FileExtension = fileExtension;
			MimeType = mimeType;
		}

		public string Name { get; }

		public string FileExtension { get; }

		public string MimeType { get; }

		public static ContentType Parse(string contentTypeString)
		{
			return ContentType.SupportedContentTypes2.First(x => x.Name.ToLower().Equals(contentTypeString.ToLower()));
		}
	}
}