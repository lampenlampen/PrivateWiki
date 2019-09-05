using StorageBackend.PageAST;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace StorageBackend.Renderer
{
	public interface IHtmlRenderer
	{
		string RenderToHtml(IPageBlock block);

		void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer);
	}
}