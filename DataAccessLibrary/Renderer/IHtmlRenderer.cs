using DataAccessLibrary.PageAST;
using Markdig.Renderers;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer
{
	public interface IHtmlRenderer
	{
		string RenderToHtml(IPageBlock block);

		void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer);
	}
}