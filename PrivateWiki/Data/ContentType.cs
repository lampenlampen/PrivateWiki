using System;
using System.Globalization;
using System.Linq;

namespace PrivateWiki.Data
{
	public class ContentType
	{
		public static readonly ContentType Markdown = new ContentType("Markdown", ".md", "text/markdown");
		public static readonly ContentType Html = new ContentType("Html", ".html", "text/html");
		public static readonly ContentType Text = new ContentType("Text", ".txt", "text/plain");

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
			var contentTypes = AppConfig.SupportedContentTypes2;

			return contentTypes.First(x => x.Name.ToLower().Equals(contentTypeString.ToLower()));
		}
	}
	
	
}