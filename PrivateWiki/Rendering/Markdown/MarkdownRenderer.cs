using System.Threading.Tasks;
using PrivateWiki.Rendering.Markdown.Markdig;

namespace PrivateWiki.Rendering.Markdown
{
	public class MarkdownRenderer : IPageRenderer
	{
		private readonly Markdig.Markdig _renderer;

		public MarkdownRenderer(Markdig.Markdig renderer)
		{
			_renderer = renderer;
		}

		public Task<string> RenderToHtmlAsync(string content)
		{
			return _renderer.ToHtmlAsync(content);
		}

		public string RenderToHtml(string content)
		{
			
		}
	}
}