using System;
using System.Reactive;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.ViewModels
{
	public interface IContentPageViewerViewModel
	{
		IObservable<Path> OnWikiLinkClicked { get; }

		IObserver<Unit> ScrollToTop { get; }

		IObservable<KeyboardShortcut> OnKeyPressed { get; }
	}
}