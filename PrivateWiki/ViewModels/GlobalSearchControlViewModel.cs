using System;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using DynamicData;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using ReactiveUI;

namespace PrivateWiki.ViewModels
{
	public class GlobalSearchControlViewModel : ReactiveObject
	{
		private readonly IPageBackendService _backend;

		private readonly ISourceCache<GenericPage, Guid> _pages;

		public readonly ReadOnlyObservableCollection<GenericPage> FilteredPages;

		private string _searchQuery = "";

		public string SearchQuery
		{
			get => _searchQuery;
			set => this.RaiseAndSetIfChanged(ref _searchQuery, value);
		}

		private IObservable<Func<GenericPage, bool>> observableSearchQuery;


		public ReactiveCommand<GenericPage, Unit> NavigateToPage { get; }

		public ReactiveCommand<KeyboardShortcut, Unit> KeyPressed { get; }

		public ReactiveCommand<Unit, Unit> HideSearchWindow { get; }

		public ReactiveCommand<GenericPage, Unit> PageSelected { get; }

		private readonly ISubject<Unit> _onClose;
		public IObservable<Unit> OnClose => _onClose;

		private readonly ISubject<GenericPage> _onPageSelected;

		public IObservable<GenericPage> OnPageSelected => _onPageSelected;

		public GlobalSearchControlViewModel()
		{
			_backend = Application.Instance.Container.GetInstance<IPageBackendService>();
			_pages = new SourceCache<GenericPage, Guid>(x => x.Id);

			NavigateToPage = ReactiveCommand.CreateFromTask<GenericPage>(NavigateToPageAsync);
			KeyPressed = ReactiveCommand.CreateFromTask<KeyboardShortcut>(KeyPressedAsync);
			HideSearchWindow = ReactiveCommand.Create<Unit>(x => _onClose.OnNext(x));
			PageSelected = ReactiveCommand.CreateFromTask<GenericPage>(NavigateToPageAsync);

			_onClose = new Subject<Unit>();
			_onPageSelected = new Subject<GenericPage>();

			observableSearchQuery = this.WhenAnyValue(vm => vm.SearchQuery).Select<string, Func<GenericPage, bool>>(searchQuery => page => page.Path.FullPath.Contains(searchQuery));

			var a = _pages.Connect().Filter(observableSearchQuery)
				.ObserveOn(RxApp.MainThreadScheduler)
				.Bind(out FilteredPages)
				.Subscribe();

			LoadData();
		}

		private async Task LoadData()
		{
			_pages.Clear();

			var pages = await _backend.GetAllPagesAsync();

			foreach (var page in pages) _pages.AddOrUpdate(page);
		}

		private async Task NavigateToPageAsync(GenericPage page)
		{
			_onPageSelected.OnNext(page);
		}

		private async Task KeyPressedAsync(KeyboardShortcut key)
		{
			switch (key)
			{
				case KeyboardShortcut.Escape:
					_onClose.OnNext(Unit.Default);
					break;
				default:
					throw new ArgumentOutOfRangeException(nameof(key), key, null);
			}
		}
	}
}