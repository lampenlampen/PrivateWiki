namespace PrivateWiki.Rendering.Markdown.Markdig
{
	internal interface IPageRenderer
	{
		string RenderToHtml(string content);
	}
}