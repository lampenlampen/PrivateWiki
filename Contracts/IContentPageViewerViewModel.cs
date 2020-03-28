using System;
using System.IO;
using System.Reactive.Subjects;
using Path = Models.Pages.Path;

namespace Contracts
{
	public interface IContentPageViewerViewModel
	{
		IObservable<Path> OnWikiLinkClicked { get; }
	}
}