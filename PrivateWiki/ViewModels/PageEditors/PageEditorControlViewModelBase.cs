using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NLog;
using PrivateWiki.DataModels.Pages;
using ReactiveUI;

namespace PrivateWiki.ViewModels.PageEditors
{
	public abstract class PageEditorControlViewModelBase : ReactiveObject, IPageEditorControlViewModel
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private protected IPageEditorCommandBarViewModel _commandBarViewModel = null!;

		public IPageEditorCommandBarViewModel CommandBarViewModel
		{
			get => _commandBarViewModel;
			set => this.RaiseAndSetIfChanged(ref _commandBarViewModel, value);
		}

		private protected GenericPage _page;

		public GenericPage Page
		{
			get => _page;
			set => this.RaiseAndSetIfChanged(ref _page, value);
		}

		private protected string _content = string.Empty;

		public string Content
		{
			get => _content;
			set => this.RaiseAndSetIfChanged(ref _content, value);
		}

		private IList<Label> _labels;

		public IList<Label> Labels
		{
			get => _labels;
			set => this.RaiseAndSetIfChanged(ref _labels, value);
		}

		public IObservable<GenericPage> OnSavePage => _onSavePage;
		private protected ISubject<GenericPage> _onSavePage;

		public IObservable<Unit> OnAbort { get; }
		public IObservable<Unit> OnOpenInExternalEditor { get; }
		public IObservable<Unit> OnDelete { get; }

		protected PageEditorControlViewModelBase()
		{
			CommandBarViewModel = new PageEditorCommandBarViewModel();

			SavePage = ReactiveCommand.CreateFromTask(SavePageAsync);

			OnAbort = CommandBarViewModel.OnAbort.AsObservable();
			_onSavePage = new Subject<GenericPage>();

			CommandBarViewModel.OnSave.InvokeCommand(this, x => x.SavePage);
			OnOpenInExternalEditor = CommandBarViewModel.OnOpenInExternalEditor.AsObservable();
			OnDelete = CommandBarViewModel.OnDelete.AsObservable();

			this.WhenAnyValue(x => x.Page)
				.WhereNotNull()
				.Subscribe(x => Labels = x.Labels);
		}

		public ReactiveCommand<Unit, Unit> SavePage { get; }

		private protected abstract Task SavePageAsync();
	}
}