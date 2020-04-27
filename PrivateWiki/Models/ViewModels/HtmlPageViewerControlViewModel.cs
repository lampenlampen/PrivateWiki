using System;
using System.Reactive;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Xaml.Controls;
using NLog;
using PrivateWiki.Models.Pages;
using PrivateWiki.Rendering;
using ReactiveUI;

namespace PrivateWiki.Models.ViewModels
{
	public class HtmlPageViewerControlViewModel : ReactiveObject, IContentPageViewerViewModel
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private ISubject<Path> _onWikiLinkClicked { get; }
		public IObservable<Path> OnWikiLinkClicked => _onWikiLinkClicked;

		private GenericPage _page;

		public GenericPage Page
		{
			get => _page;
			set => this.RaiseAndSetIfChanged(ref _page, value);
		}

		public ReactiveCommand<Unit, string> RenderContent { get; }

		public ReactiveCommand<WebViewNavigationStartingEventArgs, Unit> NavigationStarting { get; }

		public ReactiveCommand<Path, Unit> WikilinkClicked { get; }

		public ReactiveCommand<Uri, Unit> LinkClicked { get; }

		private readonly ISubject<Unit> _onScrollToTop;
		public IObservable<Unit> OnScrollToTop => _onScrollToTop;

		public IObserver<Unit> ScrollToTop => _onScrollToTop;

		private ISubject<string> _content { get; }

		public IObservable<string> Content => _content;


		public HtmlPageViewerControlViewModel()
		{
			_content = new Subject<string>();
			_onScrollToTop = new Subject<Unit>();

			_onWikiLinkClicked = new Subject<Path>();

			RenderContent = ReactiveCommand.CreateFromTask(Render);

			WikilinkClicked = ReactiveCommand.CreateFromTask<Path>(WikiLinkClickedAsync);
			LinkClicked = ReactiveCommand.CreateFromTask<Uri>(LinkClickedAsync);
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
			Launcher.LaunchUriAsync(uri);

			return Task.FromResult(Unit.Default);
		}
	}
}