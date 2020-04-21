using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NLog;
using NLog.Fluent;
using NodaTime;
using PrivateWiki.Models.Pages;
using PrivateWiki.Models.ViewModels.PageEditors;
using PrivateWiki.StorageBackend;
using PrivateWiki.StorageBackend.SQLite;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
{
	public class PageEditorViewModel : ReactiveObject
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
		
		private GenericPage Page;

		private IPageEditorControlViewModel _pageContentViewModel;

		public IPageEditorControlViewModel PageContentViewModel
		{
			get => _pageContentViewModel;
			set => this.RaiseAndSetIfChanged(ref _pageContentViewModel, value);
		}


		public readonly ReactiveCommand<Path, Unit> ShowPage;

		public readonly ReactiveCommand<GenericPage, Unit> SavePage;

		public readonly ReactiveCommand<Unit, Unit> Abort;

		public readonly ReactiveCommand<Unit, Unit> OpenInExternalEditor;

		public readonly ReactiveCommand<Unit, Unit> DeletePage;

		private readonly Interaction<Path, bool> _confirmDelete;
		public Interaction<Path, bool> ConfirmDelete => _confirmDelete;

		private ISubject<Unit> _onAbort;
		public IObservable<Unit> OnAbort => _onAbort;

		private ISubject<Path> _onSave;
		public IObservable<Path> OnSave => _onSave;

		private ISubject<Unit> _onDelete;
		public IObservable<Unit> OnDelete => _onDelete;

		private ISubject<Unit> _onOpenInExternalEditor;
		public IObservable<Unit> OnOpenInExternalEditor => _onOpenInExternalEditor;

		public PageEditorViewModel()
		{
			
			ShowPage = ReactiveCommand.CreateFromTask<Path>(ShowPageAsync);
			SavePage = ReactiveCommand.CreateFromTask<GenericPage>(SavePageAsync);
			Abort = ReactiveCommand.CreateFromTask(AbortAsync);
			OpenInExternalEditor = ReactiveCommand.CreateFromTask(OpenInExternalEditorAsync);
			DeletePage = ReactiveCommand.CreateFromTask(DeletePageAsync);
			
			_onAbort = new Subject<Unit>();
			_onSave = new Subject<Path>();
			_onOpenInExternalEditor = new Subject<Unit>();
			_onDelete = new Subject<Unit>();

			_confirmDelete = new Interaction<Path, bool>();
			
			this.WhenAnyValue(x => x.PageContentViewModel)
				.Where(x => x != null)
				.Subscribe(x =>
				{
					x.OnAbort.InvokeCommand(this, b => b.Abort);
					x.OnSavePage.InvokeCommand(this, b => b.SavePage);
					x.OnDelete.InvokeCommand(this, b => b.DeletePage);
				});
		}

		private async Task ShowPageAsync(Path path)
		{
			var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			Page = await backend.GetPageAsync(path.FullPath);

			// Testing
			PageContentViewModel = new MarkdownPageEditorControlViewModel
			{
				Page = Page
			};
			
			
		}

		private async Task DeletePageAsync()
		{
			var isDeleteAllowed = await _confirmDelete.Handle(Page.Path);

			if (isDeleteAllowed)
			{
				Logger.Info("Delete page");
				Logger.ConditionalDebug($"Delete Page ({Page.Path.FullPath})");
				
				var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
				backend.DeletePageAsync(Page);
				
				_onDelete.OnNext(Unit.Default);
			}
		}

		private Task SavePageAsync(GenericPage page)
		{
			// Save page
			var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			backend.UpdatePage(page, PageAction.Edited);

			_onSave.OnNext(Page.Path);
			return Task.CompletedTask;
		}

		private Task AbortAsync()
		{
			_onAbort.OnNext(Unit.Default);
			return Task.CompletedTask;
		}

		private Task OpenInExternalEditorAsync()
		{
			_onOpenInExternalEditor.OnNext(Unit.Default);
			return Task.CompletedTask;
		}
	}
}