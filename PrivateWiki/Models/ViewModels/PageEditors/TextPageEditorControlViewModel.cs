using System;
using System.Reactive;
using System.Reactive.Subjects;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels.PageEditors
{
	public class TextPageEditorControlViewModel : ReactiveObject, IPageEditorControlViewModel
	{
		private readonly ISubject<Unit> _savePage;
		public IObservable<Unit> OnSavePage => _savePage;

		private readonly ISubject<Unit> _abort;
		public IObservable<Unit> OnAbort => _abort;

		private readonly ISubject<Unit> _openInExternalEditor;
		public IObservable<Unit> OnOpenInExternalEditor => _openInExternalEditor;
	}
}