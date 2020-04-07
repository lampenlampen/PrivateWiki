using System;

namespace PrivateWiki.Data
{
	public enum ContentType
	{
		Markdown,
		Html,
		Text
	}

	public class ContentType2
	{
		public static ContentType2 Markdown = new ContentType2("Markdown", ".md", "text/markdown");
		public static ContentType2 Html = new ContentType2("Html", ".html", "text/html");
		public static ContentType2 Text = new ContentType2("Text", ".txt", "text/plain");

		public ContentType2(string name, string fileExtension, string mimeType)
		{
			Name = name;
			FileExtension = fileExtension;
			MimeType = mimeType;
		}

		public string Name { get; }
		
		public string FileExtension { get; }
		
		public string MimeType { get; }
	}
	
	
}