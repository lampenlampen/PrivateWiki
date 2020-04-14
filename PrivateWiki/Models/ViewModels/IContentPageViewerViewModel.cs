using System;
using System.Reactive;
using Models.Pages;

namespace Contracts
{
	public interface IContentPageViewerViewModel
	{
		IObservable<Path> OnWikiLinkClicked { get; }

		IObserver<Unit> ScrollToTop { get; }
	}
}