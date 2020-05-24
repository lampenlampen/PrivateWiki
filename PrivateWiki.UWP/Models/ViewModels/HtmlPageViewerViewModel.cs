using PrivateWiki.Models.Pages;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
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