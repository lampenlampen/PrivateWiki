using DataAccessLibrary.PageAST;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer
{
	public interface IHtmlRenderer
	{
		string RenderToHtml(IPageBlock block);

		void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer);
	}
}