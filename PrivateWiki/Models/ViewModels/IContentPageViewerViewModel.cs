using System;
using System.Reactive;
using PrivateWiki.Models.Pages;

namespace PrivateWiki.Models.ViewModels
{
	public interface IContentPageViewerViewModel
	{
		IObservable<Path> OnWikiLinkClicked { get; }

		IObserver<Unit> ScrollToTop { get; }

		IObservable<KeyboardShortcut> OnKeyPressed { get; }
	}
}