using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using NLog;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.UWP.UI.UI.Controls;
using PrivateWiki.UWP.UI.UI.Controls.PageViewers;
using PrivateWiki.ViewModels;
using ReactiveUI;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages
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

		private ISubject<LabelId> _onLabelClicked = new Subject<LabelId>();

		public PageViewer()
		{
			this.InitializeComponent();

			ViewModel = ServiceLocator.Container.GetInstance<PageViewerViewModel>();

			this.WhenActivated(disposable =>
			{
				this.WhenAnyValue(x => x.ViewModel.CommandBarViewModel)
					.Where(x => x != null)
					.Subscribe(x => commandBar.ViewModel = x)
					.DisposeWith(disposable);

				commandBar.ShowSettings.Subscribe(_ => Frame.Navigate(typeof(SettingsPage))).DisposeWith(disposable);

				commandBar.ShowHistory.Select(_ => ViewModel.Page.Path)
					.InvokeCommand(this, x => x.ViewModel.ShowHistory).DisposeWith(disposable);

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
				ViewModel.OnCloseSearchPopup.Subscribe(_ => SearchPopup.IsOpen = false).DisposeWith(disposable);

				ViewModel.ShowPageLockedNotification.RegisterHandler(ShowPageLockedNotification)
					.DisposeWith(disposable);
				ViewModel.ShowPrintBrowserDialog.RegisterHandler(ShowPrintPdfBrowserDialog).DisposeWith(disposable);

				this.WhenAnyValue(x => x.ViewModel.PageContentViewer)
					.Where(x => x != null)
					.Subscribe(x =>
					{
						var contentPresenter = new HtmlPageViewerControl
							{ViewModel = (HtmlPageViewerControlViewModel) x};
						ContentGrid.Children.Add(contentPresenter);
					}).DisposeWith(disposable);

				LabelsView.ItemsSource = ViewModel.Labels;

				AddLabelsToPageControl.ViewModel = ViewModel.AddLabelsToPageControlVM;

				AddLabelsToPageControl.ViewModel.OnCreateNewLabel
					.Subscribe(_ => NavigateToCreateNewLabelPage())
					.DisposeWith(disposable);

				AddLabelFlyout.Events().Opened
					.Select(_ => new PageId(ViewModel.Page.Id))
					.InvokeCommand(AddLabelsToPageControl.ViewModel.PopulateForPage)
					.DisposeWith(disposable);

				AddLabelFlyout.Events().Closing
					.Subscribe(_ =>
					{
						AddLabelsToPageControl.ViewModel.SaveChanges.Execute(ViewModel.Page.PageId).Subscribe();
						ViewModel.LoadLabels.Execute().Subscribe();
					})
					.DisposeWith(disposable);

				_onLabelClicked.InvokeCommand(this, x => x.ViewModel.LabelClicked)
					.DisposeWith(disposable);

				SearchPopupContentName.ViewModel = ViewModel.SearchControlViewModel;
			});
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			string id;

			if (e.Parameter == null)
			{
				Logger.Info("Show default page.");

				// TODO make default page configurable
				id = "start";
			}
			else
			{
				id = (string) e.Parameter;
			}

			Logger.Info($"Show Page: {id}");
			ViewModel.LoadPage.Execute(id);
		}

		private void NavigateToCreateNewLabelPage()
		{
			Frame.Navigate(typeof(CreateNewLabelPage));
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
			Frame.Navigate(typeof(PageEditor), link.FullPath);
		}

		private void ShowHistory(Path link)
		{
			// TODO Show History
			//Frame.Navigate(typeof(HistoryPage), link.FullPath);
		}

		private void Search()
		{
			_centerPopup(SearchPopup);
			SearchPopup.IsOpen = true;
		}

		private async Task ShowPrintPdfBrowserDialog(InteractionContext<Path, bool> context)
		{
			var dialog = new ContentDialog
			{
				Title = ViewModel.Translations.PrintPDFDialogTitle,
				Content = ViewModel.Translations.PrintPDFDialogDescription,
				PrimaryButtonText = ViewModel.Translations.PrintPDFDialogOpenInBrowser,
				CloseButtonText = ViewModel.Translations.Close,
				DefaultButton = ContentDialogButton.Primary
			};

			var result = (await dialog.ShowAsync()) == ContentDialogResult.Primary;

			context.SetOutput(result);
		}

		private Task ShowPageLockedNotification(InteractionContext<Path, Unit> context)
		{
			App.Current.GlobalNotificationManager.ShowPageLockedNotification();

			context.SetOutput(Unit.Default);

			return Task.CompletedTask;
		}

		private void _centerPopup(Popup popup, FrameworkElement? extraElement = null)
		{
			double ratio = .6; // How much of the window the popup fills, give or take. (90%)

			Panel pnl = (Panel) popup.Parent;
			double parentHeight = pnl.ActualHeight;
			double parentWidth = pnl.ActualWidth;

			// Min 200 for each dimension.
			double width = parentWidth * ratio > 200 ? parentWidth * ratio : 200;
			double height = parentHeight * ratio > 200 ? parentHeight * ratio : 200;

			popup.Width = width;
			//popup.Height = height;

			//popup.HorizontalAlignment = HorizontalAlignment.Center;
			popup.VerticalAlignment = VerticalAlignment.Top; // <<< This is ignored?!

			// Resize the border too. Not sure how to get this "for free".
			popupTestBorder.Width = width;
			popupTestBorder.Height = height;

			// Not using this here, but if there's anything else that needs resizing, do it.
			if (extraElement != null)
			{
				extraElement.Width = width;
				extraElement.Height = height;
			}
		}

		private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
		{
			var id = ((LabelControl) sender).Id;

			_onLabelClicked.OnNext(id);
		}
	}
}