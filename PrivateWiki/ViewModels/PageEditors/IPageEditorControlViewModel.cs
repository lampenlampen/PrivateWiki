using System;
using System.Reactive;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.ViewModels.PageEditors
{
	public interface IPageEditorControlViewModel
	{
		GenericPage Page { get; set; }


		IObservable<GenericPage> OnSavePage { get; }

		IObservable<Unit> OnAbort { get; }

		IObservable<Unit> OnOpenInExternalEditor { get; }

		IObservable<Unit> OnDelete { get; }
	}
}