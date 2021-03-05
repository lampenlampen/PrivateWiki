using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using PrivateWiki.Core;
using PrivateWiki.Core.DebugMode;
using PrivateWiki.Core.Events;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.TranslationService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageViewerCommandBarViewModel : ReactiveObject
	{
		private readonly IQueryHandler<GetDebugMode, DebugMode> _debugModeQueryHandler;

		private readonly IMostRecentlyVisitedPagesService _mostRecentlyVisitedPagesService;

		private readonly IObservable<CultureChangedEventArgs> _cultureChangedEvent;

		public readonly PageViewerCommandBarResources _resources;

		public IEnumerable<MostRecentlyViewedPagesItem> MostRecentlyViewedPages => _mostRecentlyVisitedPagesService.ToList();

		public bool DevOptsEnabled => _debugModeQueryHandler.Handle(new GetDebugMode()).DebugModeEnabled;

		public ReactiveCommand<Unit, Unit> DevOptionsClick { get; }

		public ReactiveCommand<Unit, Unit> NewPageClick { get; }

		private readonly ISubject<Unit> _onDevOptionsClick;
		public IObservable<Unit> OnDevOptionsClick => _onDevOptionsClick;

		private readonly ISubject<Unit> _onNewPage;
		public IObservable<Unit> OnNewPage => _onNewPage;

		public PageViewerCommandBarViewModel(TranslationResources translationResources, IObservable<CultureChangedEventArgs> cultureChangedEvent,
			IQueryHandler<GetDebugMode, DebugMode> debugModeQueryHandler, IMostRecentlyVisitedPagesService mostRecentlyVisitedPagesService)
		{
			_cultureChangedEvent = cultureChangedEvent;
			_debugModeQueryHandler = debugModeQueryHandler;
			_mostRecentlyVisitedPagesService = mostRecentlyVisitedPagesService;

			DevOptionsClick = ReactiveCommand.Create<Unit>(x => _onDevOptionsClick.OnNext(x));
			NewPageClick = ReactiveCommand.Create<Unit>(x => _onNewPage.OnNext(x));

			_onDevOptionsClick = new Subject<Unit>();
			_onNewPage = new Subject<Unit>();

			_resources = new PageViewerCommandBarResources(translationResources);

			_cultureChangedEvent.Subscribe(x => { this.RaisePropertyChanged(nameof(_resources)); });
		}
	}

	public class PageViewerCommandBarResources
	{
		private readonly TranslationResources _translation;

		public PageViewerCommandBarResources(TranslationResources resources)
		{
			_translation = resources;
		}

		public string ToTop => _translation.GetStringResource("toTop");
		public string PDF => _translation.GetStringResource("pdf");
		public string Edit => _translation.GetStringResource("edit");
		public string Search => _translation.GetStringResource("search");
		public string History => _translation.GetStringResource("history");
		public string Fullscreen => _translation.GetStringResource("fullscreen");
		public string Export => _translation.GetStringResource("export");
		public string Import => _translation.GetStringResource("import");
		public string Settings => _translation.GetStringResource("settings");
		public string DeveloperSettings => _translation.GetStringResource("developerSettings");
		public string NewPage => _translation.GetStringResource("newPage");
	}
}