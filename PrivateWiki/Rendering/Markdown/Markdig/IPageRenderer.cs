using System.Threading.Tasks;

namespace PrivateWiki.Rendering.Markdown.Markdig
{
	internal interface IPageRenderer
	{
		Task<string> RenderToHtml(string content);
	}
}