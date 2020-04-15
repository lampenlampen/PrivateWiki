using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using NLog;
using PrivateWiki.Models.Pages;
using PrivateWiki.Models.ViewModels;
using PrivateWiki.UI.Controls.PageViewers;
using PrivateWiki.UI.Pages.ContentPages;
using ReactiveUI;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PageViewer : Page, IViewFor<PageViewerViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(ViewModel), typeof(PageViewerViewModel), typeof(PageViewer), new PropertyMetadata(null));

		public PageViewerViewModel ViewModel
		{
			get => (PageViewerViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (PageViewerViewModel) value;
		}

		#endregion

		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		public PageViewer()
		{
			this.InitializeComponent();

			ViewModel = new PageViewerViewModel();

			this.WhenActivated(disposable =>
			{
				this.WhenAnyValue(x => x.ViewModel.CommandBarViewModel)
					.Where(x => x != null)
					.Subscribe(x => commandBar.ViewModel = x)
					.DisposeWith(disposable);


				commandBar.ShowSettings.Subscribe(_ => Frame.Navigate(typeof(SettingsPage))).DisposeWith(disposable);

				commandBar.ShowHistory.Select(_ => ViewModel.Page.Path).InvokeCommand(this, x => x.ViewModel.ShowHistory).DisposeWith(disposable);

				commandBar.Edit
					.Select(_ => ViewModel.Page.Path)
					.InvokeCommand(this, x => x.ViewModel.Edit)
					.DisposeWith(disposable);

				commandBar.Export
					.Select(x => ViewModel.Page.Path)
					.InvokeCommand(this, x => x.ViewModel.Export)
					.DisposeWith(disposable);

				commandBar.Import
					.InvokeCommand(this, x => x.ViewModel.Import)
					.DisposeWith(disposable);

				commandBar.Search
					.InvokeCommand(this, x => x.ViewModel.Search)
					.DisposeWith(disposable);

				commandBar.PrintPdf
					.Select(_ => ViewModel.Page.Path)
					.InvokeCommand(this, x => x.ViewModel.PrintPage)
					.DisposeWith(disposable);

				commandBar.ToggleFullscreen
					.InvokeCommand(this, x => x.ViewModel.ToggleFullscreen)
					.DisposeWith(disposable);

				commandBar.ScrollToTop
					.InvokeCommand(this, x => x.ViewModel.ScrollToTop)
					.DisposeWith(disposable);

				commandBar.NavigateToPage2
					.Select(Path.ofLink)
					.InvokeCommand(this, x => x.ViewModel.NavigateToPage)
					.DisposeWith(disposable);

				ViewModel.OnNavigateToExistingPage.Subscribe(NavigateToPage).DisposeWith(disposable);
				ViewModel.OnNavigateToNewPage.Subscribe(NavigateToNewPage).DisposeWith(disposable);
				ViewModel.OnEditPage.Subscribe(EditPage).DisposeWith(disposable);
				ViewModel.OnNewPage.Subscribe(_ => NavigateToNewPage()).DisposeWith(disposable);
				ViewModel.OnShowHistoryPage.Subscribe(a => ShowHistory(a)).DisposeWith(disposable);
				ViewModel.OnSearch.Subscribe(_ => Search()).DisposeWith(disposable);

				ViewModel.ShowPageLockedNotification.RegisterHandler(ShowPageLockedNotification).DisposeWith(disposable);
				ViewModel.ShowPrintBrowserDialog.RegisterHandler(ShowPrintPdfBrowserDialog).DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.PageContentViewer)
					.Where(x => x != null)
					.Subscribe(x =>
					{
						var contentPresenter = new HtmlPageViewerControl {ViewModel = (HtmlPageViewerControlViewModel) x};
						ContentGrid.Children.Add(contentPresenter);
					}).DisposeWith(disposable);
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			if (e.Parameter == null) throw new ArgumentNullException(nameof(e), "No page id");
			var id = (string) e.Parameter;
			Logger.Info($"Show Page: {id}");
			ViewModel.LoadPage.Execute(id);
		}

		private void NavigateToPage(Path link)
		{
			Frame.Navigate(typeof(PageViewer), link.FullPath);
		}

		private void NavigateToNewPage(Path? link = null)
		{
			if (link != null) Frame.Navigate(typeof(NewPage), link.FullPath);
			else Frame.Navigate(typeof(NewPage));
		}

		private void EditPage(Path link)
		{
			Frame.Navigate(typeof(GenericTextPageEditor), link.FullPath);
		}

		private void ShowHistory(Path link)
		{
			Frame.Navigate(typeof(HistoryPage), link.FullPath);
		}

		private void Search()
		{
			SearchPopup.IsOpen = true;
			SearchPopup.Closed += (sender, e) =>
			{
				var link = SearchPopupContentName.SelectedPageLink;

				if (link != null)
				{
					NavigateToPage(Path.ofLink(link));
				}
			};
		}

		private async Task ShowPrintPdfBrowserDialog(InteractionContext<Path, bool> context)
		{
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

			var dialog = new ContentDialog
			{
				Title = resourceLoader.GetString("PrintPDF/Dialog/Title"),
				Content = resourceLoader.GetString("PrintPDF/Dialog/Content"),
				PrimaryButtonText = resourceLoader.GetString("PrintPdf/Dialog/OpenInBrowser"),
				CloseButtonText = resourceLoader.GetString("Close"),
				DefaultButton = ContentDialogButton.Primary
			};

			var result = (await dialog.ShowAsync()) == ContentDialogResult.Primary;

			context.SetOutput(result);
		}

		private async Task ShowPageLockedNotification(InteractionContext<Path, Unit> context)
		{
			App.Current.Manager.ShowPageLockedNotification();

			context.SetOutput(Unit.Default);
		}
	}
}