﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using NLog;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageBackendService.SQLite;
using Page = PrivateWiki.DataModels.Pages.Page;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.UI
{
	public sealed partial class SearchPopupContent : UserControl
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private IEnumerable<GenericPage> _pages;
		private ObservableCollection<GenericPage> Pages = new();

		public string SelectedPageLink { get; private set; }


		public SearchPopupContent()
		{
			InitializeComponent();
			Init();

			SearchResultsBox.ItemsSource = Pages;
			//SearchBox.ItemsSource = Pages;
		}


		private async void Init()
		{
			var backend = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			_pages = await backend.GetAllPagesAsync();
		}

		private void FilterPages(string text)
		{
			var pages = _pages.Where(p => p.Link.Contains(text));

			Pages.Clear();

			foreach (var page in pages)
			{
				Pages.Add(page);
			}
		}

		private void SearchBox_OnTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			FilterPages(SearchBox.Text);
		}

		private void SearchBox_OnQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			Logger.Debug($"Search: {SearchBox.Text}");

			var p = Parent as FrameworkElement;

			while (!(p is Popup)) p = p.Parent as FrameworkElement;

			(p as Popup).IsOpen = false;
		}

		private void SearchResultsBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var page = (Page) SearchResultsBox.SelectedItem;
			SelectedPageLink = page.Link;

			var p = Parent as FrameworkElement;

			while (!(p is Popup)) p = p.Parent as FrameworkElement;

			(p as Popup).IsOpen = false;
		}

		private void SearchBox_OnSizeChanged(object sender, SizeChangedEventArgs e)
		{
			if (e.PreviousSize.Height == 0 && e.PreviousSize.Width == 0)
			{
				SearchBox.Focus(FocusState.Programmatic);
			}
		}
	}
}