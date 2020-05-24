using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Models.Pages;
using PrivateWiki.Models.ViewModels;
using PrivateWiki.Models.ViewModels.PageEditors;
using PrivateWiki.UI.Controls.PageEditors;
using ReactiveUI;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PageEditor : Page, IViewFor<PageEditorViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(PageEditorViewModel), typeof(PageEditor), new PropertyMetadata(null));

		public PageEditorViewModel ViewModel
		{
			get => (PageEditorViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (PageEditorViewModel) value;
		}

		#endregion

		public PageEditor()
		{
			InitializeComponent();
			ViewModel = new PageEditorViewModel();

			this.WhenActivated(disposable =>
			{
				this.WhenAnyValue(x => x.ViewModel.PageContentViewModel)
					.Where(x => x != null)
					.Subscribe(ShowPage)
					.DisposeWith(disposable);

				ViewModel.ConfirmDelete.RegisterHandler(ConfirmDeleteAsync).DisposeWith(disposable);

				ViewModel.OnAbort.Subscribe(_ => Abort()).DisposeWith(disposable);
				ViewModel.OnSave.Subscribe(NavigateToCurrentPage).DisposeWith(disposable);
				ViewModel.OnDelete.Subscribe(_ => NavigateToPreviousOrDefaultPage()).DisposeWith(disposable);
				ViewModel.OnOpenInExternalEditor.Subscribe(x => Frame.Navigate(typeof(ExternalEditor), x.FullPath)).DisposeWith(disposable);
			});
		}

		private async Task ConfirmDeleteAsync(InteractionContext<Path, bool> context)
		{
			var dialog = new ContentDialog
			{
				Title = $"Delete Page {context.Input.FullPath}",
				Content =
					"Delete this page permanently. After this action there is no way of restoring the current state.",
				CloseButtonText = "Cancel",
				PrimaryButtonText = "Delete Page",
				DefaultButton = ContentDialogButton.Close
			};

			context.SetOutput(await dialog.ShowAsync() == ContentDialogResult.Primary);
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			var path = Path.ofLink((string) e.Parameter);
			ViewModel.ShowPage.Execute(path);
		}

		private void ShowPage(IPageEditorControlViewModel vm)
		{
			UserControl control = vm.Page.ContentType.MimeType switch
			{
				"text/markdown" => new MarkdownPageEditorControl {ViewModel = (MarkdownPageEditorControlViewModel) vm},
				"text/html" => new HtmlPageEditorControl {ViewModel = (HtmlPageEditorControlViewModel) vm},
				"text/plain" => new TextPageEditorControl {ViewModel = (TextPageEditorControlViewModel) vm},
				_ => new TextPageEditorControl {ViewModel = (TextPageEditorControlViewModel) vm}
			};

			PageContentControlHost.Children.Add(control);
		}

		private void Abort()
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}

		private void NavigateToCurrentPage(Path path)
		{
			Frame.Navigate(typeof(PageViewer), path.FullPath);
		}

		private void NavigateToPreviousOrDefaultPage()
		{
			Frame.Navigate(typeof(PageViewer));
		}
	}
}