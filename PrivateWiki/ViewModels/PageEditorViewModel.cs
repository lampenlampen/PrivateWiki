using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.ViewModels.PageEditors;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageEditorViewModel : ReactiveObject
	{
		private readonly IPageBackendService _backend;

		private GenericPage Page = null!;

		private IPageEditorControlViewModel? _pageContentViewModel = null!;

		public IPageEditorControlViewModel PageContentViewModel
		{
			get => _pageContentViewModel;
			set => this.RaiseAndSetIfChanged(ref _pageContentViewModel, value);
		}

		public ReactiveCommand<Path, Unit> ShowPage { get; }

		public ReactiveCommand<GenericPage, Unit> SavePage { get; }

		public ReactiveCommand<Unit, Unit> Abort { get; }

		public ReactiveCommand<Unit, Unit> OpenInExternalEditor { get; }

		public ReactiveCommand<Unit, Unit> DeletePage { get; }

		public ReactiveCommand<Unit, Unit> NavigateToCreateNewLabelPage { get; }

		public ReactiveCommand<Unit, Unit> NavigateToManageLabelsPage { get; }

		private readonly Interaction<Path, bool> _confirmDelete;
		public Interaction<Path, bool> ConfirmDelete => _confirmDelete;

		private readonly ISubject<Unit> _onAbort;
		public IObservable<Unit> OnAbort => _onAbort;

		private readonly ISubject<Path> _onSave;
		public IObservable<Path> OnSave => _onSave;

		private readonly ISubject<Unit> _onDelete;
		public IObservable<Unit> OnDelete => _onDelete;

		private readonly ISubject<Path> _onOpenInExternalEditor;
		public IObservable<Path> OnOpenInExternalEditor => _onOpenInExternalEditor;

		private readonly ISubject<Unit> _onNavigateToCreateNewLabelPage;

		public IObservable<Unit> OnNavigateToCreateNewLabelPage => _onNavigateToCreateNewLabelPage;

		private readonly ISubject<Unit> _onNavigateToManageLabelsPage;

		public IObservable<Unit> OnNavigateToManageLabelsPage => _onNavigateToManageLabelsPage;

		public PageEditorViewModel()
		{
			_backend = ServiceLocator.Container.GetInstance<IPageBackendService>();
			;

			ShowPage = ReactiveCommand.CreateFromTask<Path>(ShowPageAsync);
			SavePage = ReactiveCommand.CreateFromTask<GenericPage>(SavePageAsync);
			Abort = ReactiveCommand.CreateFromTask(AbortAsync);
			OpenInExternalEditor = ReactiveCommand.CreateFromTask(OpenInExternalEditorAsync);
			DeletePage = ReactiveCommand.CreateFromTask(DeletePageAsync);
			NavigateToCreateNewLabelPage = ReactiveCommand.Create<Unit>(x => _onNavigateToCreateNewLabelPage.OnNext(x));
			NavigateToManageLabelsPage = ReactiveCommand.Create<Unit>(x => _onNavigateToManageLabelsPage.OnNext(x));

			_onAbort = new Subject<Unit>();
			_onSave = new Subject<Path>();
			_onOpenInExternalEditor = new Subject<Path>();
			_onDelete = new Subject<Unit>();
			_onNavigateToCreateNewLabelPage = new Subject<Unit>();
			_onNavigateToManageLabelsPage = new Subject<Unit>();

			_confirmDelete = new Interaction<Path, bool>();

			this.WhenAnyValue(x => x.PageContentViewModel)
				.Where(x => x != null)
				.Subscribe(x =>
				{
					x.OnAbort.InvokeCommand(this, b => b.Abort);
					x.OnSavePage.InvokeCommand(this, b => b.SavePage);
					x.OnDelete.InvokeCommand(this, b => b.DeletePage);
					x.OnOpenInExternalEditor.InvokeCommand(this, b => b.OpenInExternalEditor);
					x.OnCreateNewLabel.InvokeCommand(this, a => a.NavigateToCreateNewLabelPage);
					x.OnManageLabels.InvokeCommand(this, a => a.NavigateToManageLabelsPage);
				});
		}

		private async Task ShowPageAsync(Path path)
		{
			Page = await _backend.GetPageAsync(path.FullPath);

			PageContentViewModel = Page.ContentType.MimeType switch
			{
				"text/markdown" => new MarkdownPageEditorControlViewModel {Page = Page},
				"text/html" => new HtmlPageEditorControlViewModel {Page = Page},
				"text/plain" => new TextPageEditorControlViewModel {Page = Page},
				_ => new TextPageEditorControlViewModel {Page = Page}
			};
		}

		private async Task DeletePageAsync()
		{
			var isDeleteAllowed = await _confirmDelete.Handle(Page.Path);

			if (isDeleteAllowed)
			{
				_backend.DeletePageAsync(Page);

				_onDelete.OnNext(Unit.Default);
			}
		}

		private async Task SavePageAsync(GenericPage page)
		{
			// Save page
			await _backend.UpdatePage(page, PageAction.Edited);

			_onSave.OnNext(Page.Path);
		}

		private Task AbortAsync()
		{
			_onAbort.OnNext(Unit.Default);
			return Task.CompletedTask;
		}

		private Task OpenInExternalEditorAsync()
		{
			_onOpenInExternalEditor.OnNext(Page.Path);

			return Task.CompletedTask;
		}
	}
}