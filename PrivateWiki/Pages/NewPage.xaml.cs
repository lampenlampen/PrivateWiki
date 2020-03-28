using JetBrains.Annotations;
using NodaTime;
using PrivateWiki.Data;
using System;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Models.Pages;
using Models.Storage;
using PrivateWiki.Models.ViewModels;
using ReactiveUI;
using StorageBackend.SQLite;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace PrivateWiki.Pages
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
		
		private string _pageId;

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
		}

		private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
		{
			GoBackRequested();
			args.Handled = true;
		}

		protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			_pageId = (string) e.Parameter;
		}

		private void CreatePage_Click([NotNull] object sender, [NotNull] RoutedEventArgs e)
		{
			NavigateToPage();
		}

		private async void ImportPage_Click(object sender, RoutedEventArgs e)
		{
			var backend = new SqLiteBackend(new SqLiteStorage("test"), SystemClock.Instance);
			var file = await FileSystemAccess.PickMarkdownFileAsync();

			if (file == null) return;

			var content = await FileIO.ReadTextAsync(file);

			var page = new GenericPage(_pageId, Guid.NewGuid(), content, "markdown", SystemClock.Instance.GetCurrentInstant(), SystemClock.Instance.GetCurrentInstant(), false);

			await backend.InsertPageAsync(page);

			NavigateToPage();
		}

		private void NavigateToPage()
		{
			Frame.Navigate(typeof(PageEditor), _pageId.ToString());
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			GoBackRequested();
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