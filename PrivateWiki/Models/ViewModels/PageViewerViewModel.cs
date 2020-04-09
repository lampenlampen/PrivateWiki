using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Windows.System;
using Contracts;
using Models.Pages;
using NLog;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Storage;
using PrivateWiki.StorageBackend.SQLite;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
{
	public class PageViewerViewModel : ReactiveObject
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private SqLiteBackend _backend;

		private IContentPageViewerViewModel _pageContentViewer;

		public IContentPageViewerViewModel PageContentViewer
		{
			get => _pageContentViewer;
			private set => this.RaiseAndSetIfChanged(ref _pageContentViewer, value);
		}

		private PageViewerCommandBarViewModel _commandBarViewModel;

		public PageViewerCommandBarViewModel CommandBarViewModel
		{
			get => _commandBarViewModel;
			private set => this.RaiseAndSetIfChanged(ref _commandBarViewModel, value);
		}

		private GenericPage _page;

		public ReactiveCommand<string, Unit> LoadPage { get; }

		public ReactiveCommand<Path, Unit> ShowHistory { get; }

		public ReactiveCommand<Path, Unit> Edit { get; }

		public ReactiveCommand<Unit, Unit> Search { get; }

		public ReactiveCommand<Path, Unit> Export { get; }

		public ReactiveCommand<Unit, Unit> Import { get; }

		public ReactiveCommand<Path, Unit> PrintPage { get; }

		public ReactiveCommand<Unit, Unit> ToggleFullscreen { get; }

		public ReactiveCommand<Unit, Unit> ScrollToTop { get; }

		public ReactiveCommand<Path, Unit> NavigateToPage { get; }

		private readonly ISubject<Path> _onNavigateToExistingPage;
		public IObservable<Path> OnNavigateToExistingPage => _onNavigateToExistingPage;

		private readonly ISubject<Path> _onNavigateToNewPage;
		public IObservable<Path> OnNavigateToNewPage => _onNavigateToNewPage;

		private readonly ISubject<Path> _onEditPage;
		public IObservable<Path> OnEditPage => _onEditPage;

		private readonly Interaction<Path, Unit> _showPageLockedNotification;
		public Interaction<Path, Unit> ShowPageLockedNotification => _showPageLockedNotification;

		private readonly ISubject<Path> _onShowHistoryPage;
		public IObservable<Path> OnShowHistoryPage => _onShowHistoryPage;

		private readonly Interaction<Path, bool> _showPrintBrowserDialog;
		public Interaction<Path, bool> ShowPrintBrowserDialog => _showPrintBrowserDialog;

		public IObservable<Unit> OnNewPage => CommandBarViewModel.OnNewPage;

		public GenericPage Page
		{
			get => _page;
			set => this.RaiseAndSetIfChanged(ref _page, value);
		}

		public PageViewerViewModel()
		{
			_backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			CommandBarViewModel = new PageViewerCommandBarViewModel();

			// Commands
			LoadPage = ReactiveCommand.CreateFromTask<string>(LoadPageAsync);
			ShowHistory = ReactiveCommand.CreateFromTask<Path>(ShowHistoryAsync);
			Edit = ReactiveCommand.CreateFromTask<Path>(EditAsync);
			Search = ReactiveCommand.CreateFromTask(SearchAsync);
			Export = ReactiveCommand.CreateFromTask<Path>(ExportAsync);
			Import = ReactiveCommand.CreateFromTask(ImportAsync);
			PrintPage = ReactiveCommand.CreateFromTask<Path>(PrintPdfAsync);
			ToggleFullscreen = ReactiveCommand.CreateFromTask(ToggleFullscreenAsync);
			ScrollToTop = ReactiveCommand.CreateFromTask(ScrollToTopAsync);
			NavigateToPage = ReactiveCommand.CreateFromTask<Path>(NavigateToPageAsync);

			// Events
			_onNavigateToExistingPage = new Subject<Path>();
			_onNavigateToNewPage = new Subject<Path>();
			_onEditPage = new Subject<Path>();
			_onShowHistoryPage = new Subject<Path>();

			// Interactions
			_showPageLockedNotification = new Interaction<Path, Unit>();
			_showPrintBrowserDialog = new Interaction<Path, bool>();
		}

		private async Task LoadPageAsync(string id)
		{
			_page = await _backend.GetPageAsync(id);

			PageContentViewer = new HtmlPageViewerControlViewModel {Page = _page};
			PageContentViewer.OnWikiLinkClicked.Subscribe(x => NavigateToPageAsync(x));
		}

		private async Task NavigateToPageAsync(Path link)
		{
			NavigationHandler.AddPage(Page);

			if (await _backend.ContainsPageAsync(link.FullPath))
			{
				_onNavigateToExistingPage.OnNext(link);
			}
			else
			{
				_onNavigateToNewPage.OnNext(link);
			}
		}

		private Task ShowHistoryAsync(Path link)
		{
			Logger.Info("Show History");
			Logger.ConditionalDebug($"Show History of {link.FullPath}");

			_onShowHistoryPage.OnNext(link);

			return Task.CompletedTask;
		}

		private Task EditAsync(Path link)
		{
			if (Page.IsLocked)
			{
				_showPageLockedNotification.Handle(link);

				Logger.Info("Page is locked and cannot be edited!");
				Logger.ConditionalDebug($"Page ({link.FullPath}) is locked!");
			}
			else
			{
				Logger.Info("Page edit.");
				Logger.ConditionalDebug($"Page ({link.FullPath}) edit.");
				_onEditPage.OnNext(link);
			}

			return Task.CompletedTask;
		}

		private Task SearchAsync()
		{
			// TODO Search
			return Task.CompletedTask;
		}

		private Task ExportAsync(Path link)
		{
			// TODO Export
			return Task.CompletedTask;
		}

		private Task ImportAsync()
		{
			//TODO Import
			return Task.CompletedTask;
		}

		private async Task PrintPdfAsync(Path link)
		{
			Logger.Info("Print PDF");

			var confirmation = await _showPrintBrowserDialog.Handle(link);

			if (confirmation)
			{
				var pageExporter = new PageExporter();
				var file = pageExporter.ExportPage(Page);

				Launcher.LaunchFileAsync(await file);
			}
		}

		private Task<Unit> ToggleFullscreenAsync()
		{
			// TODO Toggle Fullscreen
			return Task.FromResult(Unit.Default);
		}

		private Task<Unit> ScrollToTopAsync()
		{
			// TODO Scroll to top
			return Task.FromResult(Unit.Default);
		}
	}
}