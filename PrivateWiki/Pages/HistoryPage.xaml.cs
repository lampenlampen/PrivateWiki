using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Models.Pages;
using NodaTime;
using PrivateWiki.Storage;
using StorageBackend.SQLite;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HistoryPage : Page
	{
		private ObservableCollection<HistoryMarkdownPage> Pages { get; set; }

		public HistoryPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var pageLink = (string) e.Parameter;

			Init(pageLink);
		}

		private async void Init(string pageLink)
		{
			var storage = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance); 
			Pages = new ObservableCollection<HistoryMarkdownPage>(await storage.GetHistory(pageLink));

			Listview.ItemsSource = Pages;
		}

		private void CloseBtn_Click(object sender, RoutedEventArgs e)
		{
			if (Frame.CanGoBack)
			{
				Frame.GoBack();
			}
		}
	}
}
