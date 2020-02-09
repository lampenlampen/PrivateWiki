using System;
using System.Collections.Generic;
using System.Reactive;
using System.Text;
using Models.Pages;
using ReactiveUI;

namespace Models.ViewModels
{
	class HtmlPageViewerViewModel : ReactiveObject
	{
		public HtmlPage Page { get; }

		public HtmlPageViewerViewModel(HtmlPage page)
		{
			Page = page;
		}
	}
}
