using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.ContentRenderer;

namespace PrivateWiki.Dependencies.Markdown.Markdig
{
	public class MarkdigRenderer : IMarkdownRenderer
	{
		public ContentType ContentType => ContentType.Markdown;
		public string Render(string page)
		{
			throw new System.NotImplementedException();
		}
	}
}