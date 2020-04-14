using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using NodaTime;
using PrivateWiki.StorageBackend;
using PrivateWiki.StorageBackend.SQLite;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PagesSettingsPage : Page
	{
		public ObservableCollection<global::PrivateWiki.Models.Pages.Page> Pages { get; set; } = new ObservableCollection<global::PrivateWiki.Models.Pages.Page>();

		public PagesSettingsPage()
		{
			this.InitializeComponent();
			Init();
		}

		private async void Init()
		{
			var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			var pages = await backend.GetAllMarkdownPagesAsync();

			foreach (var page in pages)
			{
				Pages.Add(page);
			}
		}

		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}
	}
}