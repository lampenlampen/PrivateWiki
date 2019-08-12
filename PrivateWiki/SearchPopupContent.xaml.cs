using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using DataAccessLibrary;
using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using StorageProvider;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki
{
	public sealed partial class SearchPopupContent : UserControl
	{
		private readonly List<PageModel> _pages;
		private ObservableCollection<PageModel> Pages = new ObservableCollection<PageModel>();

		private DataAccessImpl dataAccess;


		public SearchPopupContent()
		{
			InitializeComponent();
			dataAccess = new DataAccessImpl();
			
			_pages = dataAccess.GetPages();


			SearchResultsBox.ItemsSource = Pages;
			//SearchBox.ItemsSource = Pages;
		}

		private void Filter(string text)
		{
			var pages = _pages.Where(p => p.Link.Contains(text));

			Pages = new ObservableCollection<PageModel>(pages);
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