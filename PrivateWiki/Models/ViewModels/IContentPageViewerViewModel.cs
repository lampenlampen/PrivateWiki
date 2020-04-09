using System;
using Models.Pages;

namespace Contracts
{
	public interface IContentPageViewerViewModel
	{
		IObservable<Path> OnWikiLinkClicked { get; }
	}
}