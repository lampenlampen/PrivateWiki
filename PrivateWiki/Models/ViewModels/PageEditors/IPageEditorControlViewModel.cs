using System;
using System.Reactive;

namespace PrivateWiki.Models.ViewModels.PageEditors
{
	public interface IPageEditorControlViewModel
	{
		IObservable<Unit> OnSavePage { get; }

		IObservable<Unit> OnAbort { get; }

		IObservable<Unit> OnOpenInExternalEditor { get; }
	}
}