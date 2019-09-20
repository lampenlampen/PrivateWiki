using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
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
			// Init(pageLink);
			InitTest();
		}

		private async void Init(string pageLink)
		{
			var storage = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			var oldPages = new ObservableCollection<HistoryMarkdownPage>(await storage.GetMarkdownPageHistoryAsync(pageLink));
			var actualPage = await storage.GetMarkdownPageAsync(pageLink);



			Listview.ItemsSource = Pages;
		}

		private async void InitTest()
		{
			// Create dummy data.

			var pages = new ObservableCollection<HistoryMarkdownPage>();

			var id = Guid.NewGuid();
			var id2 = Guid.NewGuid();

			var created = new CreatedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567296000),
				Content = "",
				Id = id,
				Link = "test1",
				IsLocked = false,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567468840)
			};

			var edited = new EditedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567468840),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = false,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567468840),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567555220)
			};

			var locked = new LockedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567555220),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = true,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567555220),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567641650)
			};

			var unlocked = new UnlockedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567641650),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = false,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567641650),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567728450)
			};

			var edited2 = new EditedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567728450),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg\n## Heading 2\nsdfkljnhasdf",
				Id = id,
				Link = "test1",
				IsLocked = false,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567728450),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567814570)
			};

			var deleted = new DeletedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567814570),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id,
				Link = "test1",
				IsLocked = false,
				IsDeleted = true,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567814570),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567900820)
			};

			var created2 = new CreatedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567900820),
				Content = "# Test Heading 1\ntesfsdgkjasbfadfsg",
				Id = id2,
				Link = "test1",
				IsLocked = false,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567900820),
				ValidTo = Instant.FromUnixTimeMilliseconds(1567987200)
			};

			var edited3 = new EditedHistoryMarkdownPage
			{
				Created = Instant.FromUnixTimeMilliseconds(1567296000),
				ValidFrom = Instant.FromUnixTimeMilliseconds(1567987200),
				Content = "# asfökjhasdf",
				Id = id2,
				Link = "test1",
				IsLocked = false,
				IsDeleted = false,
				LastChanged = Instant.FromUnixTimeMilliseconds(1567987200),
				ValidTo = Instant.FromUnixTimeMilliseconds(0)
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