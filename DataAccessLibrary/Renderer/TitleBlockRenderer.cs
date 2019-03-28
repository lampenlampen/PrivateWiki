using DataAccessLibrary.PageAST;
using DataAccessLibrary.PageAST.Blocks;
using MarkdigHtmlRenderer = Markdig.Renderers.HtmlRenderer;

namespace DataAccessLibrary.Renderer
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