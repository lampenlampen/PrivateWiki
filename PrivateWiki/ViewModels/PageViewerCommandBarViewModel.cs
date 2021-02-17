using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Subjects;
using PrivateWiki.Services.DebugModeService;
using PrivateWiki.Services.MostRecentlyVisitedPageService;
using PrivateWiki.Services.TranslationService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageViewerCommandBarViewModel : ReactiveObject
	{
		private readonly IDebugModeService _debugModeService;

		private readonly IMostRecentlyVisitedPagesService _mostRecentlyVisitedPagesService;

		private readonly TranslationResources _translation;

		public readonly Translation Translations;

		public IEnumerable<MostRecentlyViewedPagesItem> MostRecentlyViewedPages => _mostRecentlyVisitedPagesService.ToList();

		public bool DevOptsEnabled => _debugModeService.RunningInDebugMode();

		public ReactiveCommand<Unit, Unit> DevOptionsClick { get; }

		public ReactiveCommand<Unit, Unit> NewPageClick { get; }

		private readonly ISubject<Unit> _onDevOptionsClick;
		public IObservable<Unit> OnDevOptionsClick => _onDevOptionsClick;

		private readonly ISubject<Unit> _onNewPage;
		public IObservable<Unit> OnNewPage => _onNewPage;


		public PageViewerCommandBarViewModel(TranslationResources translationResources)
		{
			_translation = translationResources;

			var container = Application.Instance.Container;

			_debugModeService = container.GetInstance<IDebugModeService>();
			_mostRecentlyVisitedPagesService = container.GetInstance<IMostRecentlyVisitedPagesService>();

			DevOptionsClick = ReactiveCommand.Create<Unit>(x => { _onDevOptionsClick.OnNext(x); });
			NewPageClick = ReactiveCommand.Create<Unit>(x => _onNewPage.OnNext(x));

			_onDevOptionsClick = new Subject<Unit>();
			_onNewPage = new Subject<Unit>();

			Translations = new Translation(this);
		}


		public class Translation
		{
			private readonly TranslationResources _translation;

			public Translation(PageViewerCommandBarViewModel parent)
			{
				_translation = parent._translation;
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
}