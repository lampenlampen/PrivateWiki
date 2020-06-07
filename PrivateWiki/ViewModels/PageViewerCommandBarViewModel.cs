using System;
using System.Reactive;
using System.Reactive.Subjects;
using PrivateWiki.Services.DebugModeService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class PageViewerCommandBarViewModel : ReactiveObject
	{
		private readonly IDebugModeService debug;

		public bool DevOptsEnabled => debug.RunningInDebugMode();

		public ReactiveCommand<Unit, Unit> DevOptionsClick { get; }

		public ReactiveCommand<Unit, Unit> NewPageClick { get; }

		private readonly ISubject<Unit> _onDevOptionsClick;
		public IObservable<Unit> OnDevOptionsClick => _onDevOptionsClick;

		private readonly ISubject<Unit> _onNewPage;
		public IObservable<Unit> OnNewPage => _onNewPage;


		public PageViewerCommandBarViewModel()
		{
			debug = Application.Instance.Container.GetInstance<IDebugModeService>();

			DevOptionsClick = ReactiveCommand.Create<Unit>(x => { _onDevOptionsClick.OnNext(x); });
			NewPageClick = ReactiveCommand.Create<Unit>(x => _onNewPage.OnNext(x));

			_onDevOptionsClick = new Subject<Unit>();
			_onNewPage = new Subject<Unit>();
		}
	}
}