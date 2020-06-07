using System;
using System.Reactive;

namespace PrivateWiki.ViewModels
{
	public interface IPageEditorCommandBarViewModel
	{
		public IObservable<Unit> OnSave { get; }

		public IObservable<Unit> OnAbort { get; }

		public IObservable<Unit> OnDelete { get; }

		public IObservable<Unit> OnOpenInExternalEditor { get; }
	}
}