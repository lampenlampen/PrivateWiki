using System;
using System.Reactive;
using PrivateWiki.Models.Pages;

namespace PrivateWiki.Models.ViewModels.PageEditors
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