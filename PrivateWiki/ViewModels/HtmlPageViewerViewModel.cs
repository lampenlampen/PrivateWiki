using PrivateWiki.DataModels.Pages;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class HtmlPageViewerViewModel : ReactiveObject
	{
		public string Content => _page.Content;

		private HtmlPage _page { get; }

		public HtmlPageViewerViewModel(HtmlPage page)
		{
			_page = page;
		}
	}
}