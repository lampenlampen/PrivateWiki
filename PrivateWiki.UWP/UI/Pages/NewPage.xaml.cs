using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Data;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;
using ReactiveUI.Validation.Extensions;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace PrivateWiki.UI.Pages
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NewPage : Page, IViewFor<NewPageViewModel>
	{
		#region ViewModel

		public static readonly DependencyProperty ViewModelProperty = DependencyProperty
			.Register(nameof(NewPageViewModel), typeof(NewPageViewModel), typeof(NewPage), new PropertyMetadata(null));

		public NewPageViewModel ViewModel
		{
			get => (NewPageViewModel) GetValue(ViewModelProperty);
			set => SetValue(ViewModelProperty, value);
		}

		object IViewFor.ViewModel
		{
			get => ViewModel;
			set => ViewModel = (NewPageViewModel) value;
		}

		#endregion

		public NewPage()
		{
			InitializeComponent();

			ViewModel = new NewPageViewModel();

			KeyboardAccelerator GoBack = new KeyboardAccelerator();
			GoBack.Key = VirtualKey.GoBack;
			GoBack.Invoked += BackInvoked;
			KeyboardAccelerator AltLeft = new KeyboardAccelerator();
			AltLeft.Key = VirtualKey.Left;
			AltLeft.Invoked += BackInvoked;
			this.KeyboardAccelerators.Add(GoBack);
			this.KeyboardAccelerators.Add(AltLeft);
			// ALT routes here
			AltLeft.Modifiers = VirtualKeyModifiers.Menu;

			this.WhenActivated(disposable =>
			{
				this.Bind(ViewModel,
						vm => vm.ContentType,
						view => view.ContentTypeBox.SelectedItem,
						x => x,
						x => (ContentType) x)
					.DisposeWith(disposable);

				this.Bind(ViewModel,
						vm => vm.LinkString,
						view => view.PathBox.Text)
					.DisposeWith(disposable);

				CloseBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.GoBack)
					.DisposeWith(disposable);

				CreatePageBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.CreateNewPage)
					.DisposeWith(disposable);

				ImportPageBtn.Events().Click
					.Select(_ => Unit.Default)
					.InvokeCommand(ViewModel.ImportPage)
					.DisposeWith(disposable);

				ViewModel.CreateNewPage.CanExecute
					.Subscribe(x => { CreatePageBtn.IsEnabled = x; })
					.DisposeWith(disposable);

				ViewModel.ImportPage.CanExecute
					.Subscribe(x => { ImportPageBtn.IsEnabled = x; })
					.DisposeWith(disposable);

				ViewModel.OnGoBack.Subscribe(_ => GoBackRequested()).DisposeWith(disposable);
				ViewModel.OnCreateNewPage.ObserveOnDispatcher().Subscribe(_ => NavigateToPageEditor()).DisposeWith(disposable);
				ViewModel.OnImportNewPage.ObserveOnDispatcher().Subscribe(_ => NavigateToPageViewer()).DisposeWith(disposable);

				header1.Text = string.IsNullOrEmpty(ViewModel.LinkString) ? "Create a new page." : "This Page doesn't exist. Create it.";
				ContentTypeBox.ItemsSource = ViewModel.SupportedContentTypes;

				this.BindValidation(ViewModel,
						vm => vm.LinkValidationRule,
						v => v.ErrorLinkTextBlock.Text)
					.DisposeWith(disposable);

				this.BindValidation(ViewModel,
						vm => vm.ContentTypeValidationRule,
						v => v.ErrorContentTypeTextBlock.Text)
					.DisposeWith(disposable);
			});
		}

		private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
		{
			GoBackRequested();
			args.Handled = true;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			var link = (string) e.Parameter;

			if (link != null) ViewModel.LinkString = link;
		}

		private void NavigateToPageEditor()
		{
			Frame.Navigate(typeof(PageEditor), ViewModel.LinkString);
		}

		private void NavigateToPageViewer()
		{
			Frame.Navigate(typeof(PageViewer), ViewModel.LinkString);
		}

		private bool GoBackRequested()
		{
			if (Frame.CanGoBack)
			{
				Frame.GoBack();
				return true;
			}

			return false;
		}
	}
}