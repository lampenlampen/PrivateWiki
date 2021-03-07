using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.ApplicationLauncherService;
using PrivateWiki.Services.RenderingService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class HtmlPageViewerControlViewModel : ReactiveObject, IContentPageViewerViewModel
	{
		private ISubject<Path> _onWikiLinkClicked { get; }
		public IObservable<Path> OnWikiLinkClicked => _onWikiLinkClicked;

		private GenericPage _page;

		public GenericPage Page
		{
			get => _page;
			set => this.RaiseAndSetIfChanged(ref _page, value);
		}

		public ReactiveCommand<Unit, string> RenderContent { get; }

		public ReactiveCommand<Path, Unit> WikilinkClicked { get; }

		public ReactiveCommand<Uri, Unit> LinkClicked { get; }

		public ReactiveCommand<KeyboardShortcut, Unit> KeyPressed { get; }

		private readonly ISubject<Unit> _onScrollToTop;
		public IObservable<Unit> OnScrollToTop => _onScrollToTop;

		public IObserver<Unit> ScrollToTop => _onScrollToTop;

		private ISubject<string> _content { get; }

		public IObservable<string> Content => _content;

		public IObservable<KeyboardShortcut> OnKeyPressed => _onKeyPressed;
		private readonly Subject<KeyboardShortcut> _onKeyPressed;


		public HtmlPageViewerControlViewModel()
		{
			_content = new Subject<string>();
			_onScrollToTop = new Subject<Unit>();
			_onKeyPressed = new Subject<KeyboardShortcut>();

			_onWikiLinkClicked = new Subject<Path>();

			RenderContent = ReactiveCommand.CreateFromTask(Render);
			WikilinkClicked = ReactiveCommand.CreateFromTask<Path>(WikiLinkClickedAsync);
			LinkClicked = ReactiveCommand.CreateFromTask<Uri>(LinkClickedAsync);
			KeyPressed = ReactiveCommand.Create<KeyboardShortcut>(x => _onKeyPressed.OnNext(x));
		}

		private Task<string> Render()
		{
			var renderer = new ContentRenderer();
			return renderer.RenderPageAsync(_page);
		}

		private Task<Unit> WikiLinkClickedAsync(Path link)
		{
			_onWikiLinkClicked.OnNext(link);

			return Task.FromResult(Unit.Default);
		}

		private Task<Unit> LinkClickedAsync(Uri uri)
		{
			var launcher = ServiceLocator.Container.GetInstance<IApplicationLauncherService>();

			launcher.LaunchUriAsync(uri);

			return Task.FromResult(Unit.Default);
		}
	}
}