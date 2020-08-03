using System.Threading.Tasks;
using PrivateWiki.Rendering.Markdown.Markdig;

namespace PrivateWiki.Rendering.Markdown
{
	public class MarkdownRenderer : IPageRenderer
	{
		private readonly Markdig.Markdig _renderer = new Markdig.Markdig();

		public Task<string> RenderToHtml(string content)
		{
			return _renderer.ToHtml(content);
		}
	}
}