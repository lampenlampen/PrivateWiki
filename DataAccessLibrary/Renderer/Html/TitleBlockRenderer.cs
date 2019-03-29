using DataAccessLibrary.PageAST;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer.Html
{
	public class TitleBlockRenderer: IHtmlRenderer
	{
		public string RenderToHtml(IPageBlock block)
		{
			throw new System.NotImplementedException();
		}

		public void RenderToHtml(IPageBlock block, MarkdigHtmlRenderer renderer)
		{
			throw new System.NotImplementedException();
		}
	}
}