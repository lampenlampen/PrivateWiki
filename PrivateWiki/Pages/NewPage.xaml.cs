using System;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using PrivateWiki.Data;
using StorageProvider;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
namespace PrivateWiki.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class NewPage : Page
	{
		[CanBeNull] private string _pageId;

		public NewPage()
		{
			this.InitializeComponent();
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
			var file = await MediaAccess.PickMarkdownFileAsync();

			if (file == null) return;

			var page = ContentPage.Create(_pageId);

			page.Content = await FileIO.ReadTextAsync(file);

			new ContentPageProvider().InsertContentPage(page);

			NavigateToPage();
		}

		private void NavigateToPage()
		{
			Frame.Navigate(typeof(PageEditor), _pageId);
		}
	}
}