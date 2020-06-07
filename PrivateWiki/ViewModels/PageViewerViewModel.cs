using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using NLog;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageViewerViewModel : ReactiveObject
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly IPageBackendService _backend;

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

		public GenericPage Page
		{
			get => _page;
			private set => this.RaiseAndSetIfChanged(ref _page, value);
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

		public IObservable<Unit> OnSearch => _onSearch;
		private readonly ISubject<Unit> _onSearch;

		public PageViewerViewModel()
		{
			_backend = Application.Instance.Container.GetInstance<IPageBackendService>();
			;
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
			_onSearch = new Subject<Unit>();

			// Interactions
			_showPageLockedNotification = new Interaction<Path, Unit>();
			_showPrintBrowserDialog = new Interaction<Path, bool>();
		}

		private async Task LoadPageAsync(string id)
		{
			Page = await _backend.GetPageAsync(id);

			PageContentViewer = new HtmlPageViewerControlViewModel {Page = Page};
			PageContentViewer.OnWikiLinkClicked.Subscribe(x => NavigateToPageAsync(x));
			PageContentViewer.OnKeyPressed.Subscribe(KeyPressed);
		}

		private async void KeyPressed(KeyboardShortcut key)
		{
			switch (key)
			{
				case KeyboardShortcut.EditPage:
					await Edit.Execute(Page.Path);
					break;
				case KeyboardShortcut.SearchPage:
					await Search.Execute();
					break;
				case KeyboardShortcut.PrintPdfPage:
					await PrintPage.Execute();
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(key), key, null);
			}
		}

		private async Task NavigateToPageAsync(Path link)
		{
			// TODO NavigationHandler
			//NavigationHandler.AddPage(Page);

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

			//App.Current.Manager.ShowNotImplementedNotification();
			_onShowHistoryPage.OnNext(link);

			return Task.CompletedTask;
		}

		private async Task EditAsync(Path link)
		{
			if (Page.IsLocked)
			{
				await _showPageLockedNotification.Handle(link);

				Logger.Info("Page is locked and cannot be edited!");
				Logger.ConditionalDebug($"Page ({link.FullPath}) is locked!");
			}
			else
			{
				Logger.Info("Page edit.");
				Logger.ConditionalDebug($"Page ({link.FullPath}) edit.");
				_onEditPage.OnNext(link);
			}
		}

		private Task SearchAsync()
		{
			_onSearch.OnNext(Unit.Default);

			return Task.CompletedTask;
		}

		private Task ExportAsync(Path link)
		{
			// TODO Export
			Application.Instance.GlobalNotificationManager.ShowNotImplementedNotification();

			return Task.CompletedTask;
		}

		private Task ImportAsync()
		{
			//TODO Import
			Application.Instance.GlobalNotificationManager.ShowNotImplementedNotification();

			return Task.CompletedTask;
		}

		private async Task PrintPdfAsync(Path link)
		{
			Logger.Info("Print PDF");

			var confirmation = await _showPrintBrowserDialog.Handle(link);

			if (confirmation)
			{
				// TODO Export Page
				/*
				var pageExporter = new PageExporter();
				var file = pageExporter.ExportPage(Page);

				var launcher = new Launcher(Application.Instance.Launcher);
				launcher.LaunchFileAsync(await file);
				*/
			}
		}

		private Task ToggleFullscreenAsync()
		{
			// TODO Toggle Fullscreen
			Application.Instance.GlobalNotificationManager.ShowNotImplementedNotification();
			return Task.CompletedTask;
		}

		private Task ScrollToTopAsync()
		{
			PageContentViewer.ScrollToTop.OnNext(Unit.Default);

			return Task.CompletedTask;
		}
	}
}