using PrivateWiki.Data.DataAccess;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Contracts.Storage;
using Models.Pages;
using Models.Storage;
using NodaTime;
using StorageBackend;
using StorageBackend.SQLite;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki
{
	public sealed partial class SearchPopupContent : UserControl
	{
		private IEnumerable<MarkdownPage> _pages;
		private ObservableCollection<MarkdownPage> Pages = new ObservableCollection<MarkdownPage>();


		public SearchPopupContent()
		{
			InitializeComponent();
			var backend = new SqLiteBackend(new SqLiteStorage("test"), SystemClock.Instance);


			SearchResultsBox.ItemsSource = Pages;
			//SearchBox.ItemsSource = Pages;
		}

		private async void LoadPagesAsync(IMarkdownPageStorage storage)
		{
			_pages = await storage.GetAllMarkdownPagesAsync();
		}

		private void Filter(string text)
		{
			var pages = _pages.Where(p => p.Link.Contains(text));

			Pages = new ObservableCollection<MarkdownPage>(pages);
		}

		private void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			Filter(SearchBox.Text);
		}

		private void SearchBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			Debug.WriteLine($"Search: {SearchBox.Text}");

			var p = Parent as FrameworkElement;

			while (!(p is Popup)) p = p.Parent as FrameworkElement;

			(p as Popup).IsOpen = false;
		}
	}
}