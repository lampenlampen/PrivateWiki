using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.Backends;
using PrivateWiki.Services.GlobalNotificationService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.TranslationService;
using PrivateWiki.ViewModels.Controls;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageViewerViewModel : ReactiveObject
	{
		private readonly IMostRecentlyVisitedPagesService _mostRecentlyVisitedPagesService;
		private readonly IPageLabelsBackend _pageLabelsBackend;
		private readonly ILabelBackend _labelBackend;
		private readonly TranslationManager _translation;
		private readonly IGlobalNotificationManager _globalNotificationManager;

		private readonly IPageBackendService _backend;

		public readonly Translation Translations;

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

		public GlobalSearchControlViewModel SearchControlViewModel { get; }

		private AddLabelsToPageControlViewModel _addLabelsToPageControlVM;

		public AddLabelsToPageControlViewModel AddLabelsToPageControlVM
		{
			get => _addLabelsToPageControlVM;
			private set => this.RaiseAndSetIfChanged(ref _addLabelsToPageControlVM, value);
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

		public ReactiveCommand<Unit, Unit> LoadLabels { get; }

		public ReactiveCommand<LabelId, Unit> LabelClicked { get; }

		private readonly ISubject<Path> _onNavigateToExistingPage;
		public IObservable<Path> OnNavigateToExistingPage => _onNavigateToExistingPage;

		private readonly ISubject<Path?> _onNavigateToNewPage;
		public IObservable<Path?> OnNavigateToNewPage => _onNavigateToNewPage;

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

		public IObservable<Unit> OnCloseSearchPopup;

		private readonly ISourceCache<Label, LabelId> _labels = new SourceCache<Label, LabelId>(label => label.LabelId);

		public readonly ObservableCollection<Label> Labels = new ObservableCollectionExtended<Label>();

		public PageViewerViewModel(
			IPageBackendService pageBackendService,
			IPageLabelsBackend pageLabelsBackend,
			ILabelBackend labelBackend,
			IMostRecentlyVisitedPagesService mostRecentlyVisitedPagesService,
			TranslationManager translationManager,
			PageViewerCommandBarViewModel pageViewerCommandBarViewModel,
			GlobalSearchControlViewModel globalSearchControlViewModel,
			AddLabelsToPageControlViewModel addLabelsToPageControlVm,
			IGlobalNotificationManager globalNotificationManager)
		{
			_backend = pageBackendService;
			_pageLabelsBackend = pageLabelsBackend;
			_labelBackend = labelBackend;
			_mostRecentlyVisitedPagesService = mostRecentlyVisitedPagesService;
			_translation = translationManager;
			CommandBarViewModel = pageViewerCommandBarViewModel;
			SearchControlViewModel = globalSearchControlViewModel;
			AddLabelsToPageControlVM = addLabelsToPageControlVm;
			_globalNotificationManager = globalNotificationManager;

			Translations = new Translation(this);

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
			LoadLabels = ReactiveCommand.CreateFromTask(LoadLabelsAsync, outputScheduler: RxApp.TaskpoolScheduler);
			LabelClicked = ReactiveCommand.CreateFromTask<LabelId>(LabelClickedAsync);

			// Events
			_onNavigateToExistingPage = new Subject<Path>();
			_onNavigateToNewPage = new Subject<Path?>();
			_onEditPage = new Subject<Path>();
			_onShowHistoryPage = new Subject<Path>();
			_onSearch = new Subject<Unit>();

			// Interactions
			_showPageLockedNotification = new Interaction<Path, Unit>();
			_showPrintBrowserDialog = new Interaction<Path, bool>();

			this.WhenAnyValue(x => x.SearchControlViewModel)
				.Subscribe(x => { OnCloseSearchPopup = x.OnClose; });

			this.WhenAnyValue(x => x.Page)
				.WhereNotNull()
				.Do(x => LoadPageLabelsAsync(x.Id))
				.Subscribe();

			SearchControlViewModel.OnPageSelected.Subscribe(async page => await NavigateToPageAsync(page.Path));

			_labels.Connect()
				.ObserveOn(RxApp.MainThreadScheduler)
				.Bind((IObservableCollection<Label>) Labels)
				.Subscribe();
		}

		private async Task LoadPageAsync(string id)
		{
			Page = await _backend.GetPageAsync(id);

			PageContentViewer = new HtmlPageViewerControlViewModel {Page = Page};
			PageContentViewer.OnWikiLinkClicked.Subscribe(async x => await NavigateToPageAsync(x));
			PageContentViewer.OnKeyPressed.Subscribe(KeyPressed);
		}

		private async Task RenderPageAsync(GenericPage page)
		{
			PageContentViewer = new HtmlPageViewerControlViewModel {Page = page};
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
				case KeyboardShortcut.NewPage:
					_onNavigateToNewPage.OnNext(null);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(key), key, null);
			}
		}

		private async Task NavigateToPageAsync(Path link)
		{
			// TODO NavigationHandler
			_mostRecentlyVisitedPagesService.AddPage(Page);

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
			_onShowHistoryPage.OnNext(link);

			return Task.CompletedTask;
		}

		private async Task EditAsync(Path link)
		{
			if (Page.IsLocked)
			{
				await _showPageLockedNotification.Handle(link);
			}
			else
			{
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
			_globalNotificationManager.ShowNotImplementedNotification();

			return Task.CompletedTask;
		}

		private Task ImportAsync()
		{
			//TODO Import
			_globalNotificationManager.ShowNotImplementedNotification();

			return Task.CompletedTask;
		}

		private async Task PrintPdfAsync(Path link)
		{
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
			_globalNotificationManager.ShowNotImplementedNotification();
			return Task.CompletedTask;
		}

		private Task ScrollToTopAsync()
		{
			PageContentViewer.ScrollToTop.OnNext(Unit.Default);

			return Task.CompletedTask;
		}

		private async Task LoadPageLabelsAsync(Guid pageId)
		{
			var labelIds = await _pageLabelsBackend.GetLabelIdsForPageId(pageId);

			var labels = new List<Label>();

			foreach (var labelId in labelIds)
			{
				var label = await _labelBackend.GetLabelAsync(labelId);

				if (label.IsSuccess)
				{
					labels.Add(label.Value);
				}

				// TODO Failed case, log
			}

			_labels.Edit(updater =>
			{
				updater.Clear();
				updater.AddOrUpdate(labels);
			});
		}

		private Task LabelClickedAsync(LabelId id)
		{
			_globalNotificationManager.ShowNotImplementedNotification();

			return Task.CompletedTask;
		}

		private Task LoadLabelsAsync() => LoadPageLabelsAsync(Page.Id);

		public class Translation
		{
			private readonly TranslationManager _translation;

			public Translation(PageViewerViewModel parent)
			{
				_translation = parent._translation;
			}

			public string Labels => _translation.GetStringResource("labels");
			public string TableOfContents => _translation.GetStringResource("table_of_contents");
			public string Edit => _translation.GetStringResource("edit");
			public string PrintPDFDialogDescription => _translation.GetStringResource("printPDFDialogDescription");
			public string PrintPDFDialogTitle => _translation.GetStringResource("printPDFDialogTitle");
			public string PrintPDFDialogOpenInBrowser => _translation.GetStringResource("printPDFDialogOpenInBrowser");
			public string Close => _translation.GetStringResource("close");

			public string Test => _translation.GetStringResource("invariant_only_test");
		}
	}
}