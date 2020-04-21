using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using PrivateWiki.Models.Pages;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels.PageEditors
{
	public class TextPageEditorControlViewModel : ReactiveObject, IPageEditorControlViewModel
	{
		private IPageEditorCommandBarViewModel _commandBarViewModel = null!;

		public IPageEditorCommandBarViewModel CommandBarViewModel
		{
			get => _commandBarViewModel;
			private set => this.RaiseAndSetIfChanged(ref _commandBarViewModel, value);
		}

		public GenericPage Page { get; set; }
		public IObservable<GenericPage> OnSavePage { get; }

		public IObservable<Unit> OnAbort { get; }

		public IObservable<Unit> OnOpenInExternalEditor { get; }
		
		public IObservable<Unit> OnDelete { get; }

		public TextPageEditorControlViewModel()
		{
			CommandBarViewModel = new PageEditorCommandBarViewModel();

			OnAbort = CommandBarViewModel.OnAbort.AsObservable();
			// OnSavePage = CommandBarViewModel.OnSave.AsObservable();
			OnOpenInExternalEditor = CommandBarViewModel.OnOpenInExternalEditor.AsObservable();
			OnDelete = CommandBarViewModel.OnDelete.AsObservable();
		}
	}
}