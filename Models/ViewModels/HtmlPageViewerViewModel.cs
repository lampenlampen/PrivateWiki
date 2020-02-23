using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Models.Pages;
using ReactiveUI;

namespace Models.ViewModels
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
