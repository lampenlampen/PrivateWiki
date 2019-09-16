using JetBrains.Annotations;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using System;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using Models.Pages;
using Models.Storage;
using StorageBackend;
using StorageBackend.SQLite;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace PrivateWiki.Pages
{
	/// <summary>
	///     An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NewPage : Page
	{
		[CanBeNull] private string _pageId;

		public NewPage()
		{
			InitializeComponent();

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
			var file = await MediaAccess.PickMarkdownFileAsync();

			if (file == null) return;

			var content = await FileIO.ReadTextAsync(file);

			var page = new MarkdownPage(Guid.NewGuid(), _pageId, content, SystemClock.Instance.GetCurrentInstant(), SystemClock.Instance.GetCurrentInstant(), false);

			await backend.InsertMarkdownPageAsync(page);

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