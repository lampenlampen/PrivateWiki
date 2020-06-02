using System;
using System.Reactive;
using System.Reactive.Subjects;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
{
	public class PageEditorCommandBarViewModel : ReactiveObject, IPageEditorCommandBarViewModel
	{
		public ReactiveCommand<Unit, Unit> Save { get; }

		public ReactiveCommand<Unit, Unit> Abort { get; }

		public ReactiveCommand<Unit, Unit> Delete { get; }

		public ReactiveCommand<Unit, Unit> OpenInExternalEditor { get; }

		public IObservable<Unit> OnSave => Save;

		public IObservable<Unit> OnAbort => Abort;
		private readonly ISubject<Unit> _onAbort;

		public IObservable<Unit> OnDelete => Delete;

		public IObservable<Unit> OnOpenInExternalEditor => OpenInExternalEditor;

		public PageEditorCommandBarViewModel()
		{
			Save = ReactiveCommand.Create<Unit>(_ => { });
			Abort = ReactiveCommand.Create<Unit>(x => { });
			Delete = ReactiveCommand.Create<Unit>(_ => { });
			OpenInExternalEditor = ReactiveCommand.Create<Unit>(_ => { });
		}
	}
}