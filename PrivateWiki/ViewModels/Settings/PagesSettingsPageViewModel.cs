using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using ReactiveUI;

namespace PrivateWiki.ViewModels.Settings
{
	public class PagesSettingsPageViewModel : ReactiveObject
	{
		private readonly IPageBackendService _pageBackend;

		public readonly ObservableCollection<GenericPage> Pages = new ObservableCollection<GenericPage>();

		public readonly ReactiveCommand<Unit, Unit> LoadPages;

		public PagesSettingsPageViewModel()
		{
			_pageBackend = ServiceLocator.Container.GetInstance<IPageBackendService>();

			LoadPages = ReactiveCommand.CreateFromTask(LoadPagesAsync);
		}

		private async Task LoadPagesAsync()
		{
			var pages = await _pageBackend.GetAllPagesAsync();

			foreach (var page in pages)
			{
				Pages.Add(page);
			}
		}
	}
}