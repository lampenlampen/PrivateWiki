using DataAccessLibrary.PageAST;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer.HtmlRenderer
{
	public abstract class HtmlRenderer : IHtmlRenderer
	{
		public MarkdigHtmlRenderer _renderer;
		
		public abstract string RenderToHtml(IPageBlock block);
		
		public abstract void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer);
	}
}