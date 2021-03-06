﻿using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;
using NodaTime;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageBackendService;
using PrivateWiki.Services.StorageBackendService.SQLite;
using Page = Windows.UI.Xaml.Controls.Page;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class HistoryPage : Page
	{
		private ObservableCollection<GenericPageHistory> Pages { get; set; }

		private string _link;

		public HistoryPage()
		{
			this.InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var pageLink = (string) e.Parameter;
			_link = pageLink;

			// For developing purposes
			Init(pageLink);
			//InitTest();
		}

		private async void Init(string pageLink)
		{
			var storage = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			var pages = new ObservableCollection<GenericPageHistory>(await storage.GetPageHistoryAsync(pageLink));

			Pages = pages;
			Bindings.Update();
		}

		private async void InitTest()
		{
			// Create dummy data.

			var pages = new ObservableCollection<GenericPageHistory>();

			var id = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			var createdPage = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "",
				Id = id,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567296000)
			};

			var created = new GenericPageHistory(createdPage)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567468840),
				Action = PageAction.Created
			};

			var editedPage = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567468840)
			};

			var edited = new GenericPageHistory(editedPage)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567468840),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567555220),
				Action = PageAction.Edited
			};

			var lockedPage = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = true,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567555220),
			};

			var locked = new GenericPageHistory(lockedPage)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567555220),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567641650),
				Action = PageAction.Locked
			};

			var unlockedPage = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567641650)
			};

			var unlocked = new GenericPageHistory(unlockedPage)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567641650),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567728450),
				Action = PageAction.Unlocked
			};

			var edited2Page = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg\n## Heading 2\nsdfkljnhasdf",
				Id = id,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567728450),
			};

			var edited2 = new GenericPageHistory(edited2Page)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567728450),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567814570),
				Action = PageAction.Edited
			};

			var deletedPage = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567814570),
			};

			var deleted = new GenericPageHistory(deletedPage)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567814570),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567900820),
				Action = PageAction.Deleted
			};

			var created2Page = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id2,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567900820)
			};

			var created2 = new GenericPageHistory(created2Page)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567900820),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567987200),
				Action = PageAction.Created
			};

			var edited3Page = new GenericPage()
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "# asfökjhasdf",
				Id = id2,
				Link = "test1",
				IsLocked = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567987200)
			};

			var edited3 = new GenericPageHistory(edited3Page)
			{
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567987200),
				ValidTo = Instant.FromUnixTimeMilliseconds(0),
				Action = PageAction.Edited
			};

			pages.Add(created);
			pages.Add(edited);
			pages.Add(locked);
			pages.Add(unlocked);
			pages.Add(edited2);
			pages.Add(deleted);
			pages.Add(created2);
			pages.Add(edited3);

			Pages = pages;
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