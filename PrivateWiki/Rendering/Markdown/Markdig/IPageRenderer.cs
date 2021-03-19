using System.Threading.Tasks;

namespace PrivateWiki.Rendering.Markdown.Markdig
{
	internal interface IPageRenderer
	{
		Task<string> RenderToHtmlAsync(string content);

		public string RenderToHtml(string content);
	}
}