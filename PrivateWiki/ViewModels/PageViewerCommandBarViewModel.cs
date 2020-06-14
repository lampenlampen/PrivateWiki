using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.LastRecentlyVisitedPageService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageViewerCommandBarViewModel : ReactiveObject
	{
		private readonly IDebugModeService _debugModeService;

		private readonly IMostRecentlyVisitedPagesService _mostRecentlyVisitedPagesService;

		public IEnumerable<MostRecentlyViewedPagesItem> MostRecentlyViewedPages => _mostRecentlyVisitedPagesService.ToList();

		public bool DevOptsEnabled => _debugModeService.RunningInDebugMode();

		public ReactiveCommand<Unit, Unit> DevOptionsClick { get; }

		public ReactiveCommand<Unit, Unit> NewPageClick { get; }

		private readonly ISubject<Unit> _onDevOptionsClick;
		public IObservable<Unit> OnDevOptionsClick => _onDevOptionsClick;

		private readonly ISubject<Unit> _onNewPage;
		public IObservable<Unit> OnNewPage => _onNewPage;


		public PageViewerCommandBarViewModel()
		{
			_debugModeService = Application.Instance.Container.GetInstance<IDebugModeService>();
			_mostRecentlyVisitedPagesService = Application.Instance.Container.GetInstance<IMostRecentlyVisitedPagesService>();

			DevOptionsClick = ReactiveCommand.Create<Unit>(x => { _onDevOptionsClick.OnNext(x); });
			NewPageClick = ReactiveCommand.Create<Unit>(x => _onNewPage.OnNext(x));

			_onDevOptionsClick = new Subject<Unit>();
			_onNewPage = new Subject<Unit>();
		}
	}
}