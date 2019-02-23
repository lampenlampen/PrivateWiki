using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Windows.Storage;
using Windows.System;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using PrivateWiki.Data;
using PrivateWiki.Dialogs;
using PrivateWiki.Markdig;
using StorageProvider;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages
{
    /// <summary>
    ///     Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageViewer : Page
	{
		private readonly string CodeButtonCopy = "codeButtonCopy";

		private string contentPageId { get; set; }

		private ContentPage Page { get; set; }

		public PageViewer()
		{
			InitializeComponent();
		}

		private void WebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			var uri = args.Uri;

			// Preview Button Clicked; Do nothing
			if (uri == null || string.IsNullOrEmpty(uri.AbsoluteUri)) return;

			// WikiLink
			var splittedLink = uri.AbsoluteUri.Split(':', StringSplitOptions.RemoveEmptyEntries);
			if (splittedLink.Length >= 3)
			{
				var builder = new StringBuilder();
				builder.Append(splittedLink[2]);
				for (var i = 3; i < splittedLink.Length; i++) builder.Append($":{splittedLink[i]}");

				var wikilink = builder.ToString();

				Debug.WriteLine($"WikiLink: {wikilink}");
				args.Cancel = true;

				NavigateToPage(wikilink);
			}

			// Local Link in Document
			if (uri.AbsoluteUri.StartsWith("about:"))
			{
				Debug.WriteLine($"Local Link: {uri.AbsoluteUri}");
				return;
			}

			if (uri.AbsoluteUri.StartsWith("ms-local-stream:"))
			{
				Debug.WriteLine($"Local HtmlFile: {uri.AbsoluteUri}");
				return;
			}

			// Normal Link
			Debug.WriteLine($"Link: {uri.AbsoluteUri}");
			var success = Launcher.LaunchUriAsync(uri);
			args.Cancel = true;
		}

		protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			contentPageId = (string) e.Parameter;
			Debug.WriteLine($"Id: {contentPageId}");

			if (contentPageId == null) throw new ArgumentNullException("Page id must be nonnull!");

			ShowContentPage();
		}

		private async void ShowContentPage()
		{
			var provider = new ContentPageProvider();

			if (!provider.ContainsContentPage(contentPageId))
				if (Frame.CanGoBack)
				{
					Frame.GoBack();
					return;
				}

			Page = provider.GetContentPage(contentPageId);
			Debug.WriteLine($"Page Some: {contentPageId}");
			var parser = new MarkdigParser();


			// Show Page Title
			//PageTitle.Text = Page.Id;

			// Show Last Visited Pages
			NavigationHandler.AddPage(Page);
			Debug.WriteLine($"Last Visited Pages: {NavigationHandler.Pages.Count}");
			ShowLastVisitedPages2();

			// Show TOC
			var doc = parser.Parse(Page);
			var toc = new HeadersParser().ParseHeaders(doc);
			foreach (var header in toc)
			{
				TreeView.RootNodes.Add(header);
				header.IsExpanded = true;
			}

			// Show Page

			var html = parser.ToHtmlString(Page);
			var localFolder = ApplicationData.Current.LocalFolder;
			var mediaFolder = await localFolder.CreateFolderAsync("media", CreationCollisionOption.OpenIfExists);
			var file = await mediaFolder.CreateFileAsync("index.html", CreationCollisionOption.ReplaceExisting);
			await FileIO.WriteTextAsync(file, html);

			Webview.Navigate(new Uri("ms-appdata:///local/media/index.html"));

			// Show Tags
			if (Page.Tags != null)
				foreach (var tag in Page.Tags)
					ListView.Items.Add(tag.Name);
		}

		private void ShowLastVisitedPages()
		{
			var stack = NavigationHandler.Pages;

			if (stack.Count <= 0) return;

			switch (stack.Count)
			{
				case 4:


					break;
				case 3:
					break;
				case 2:
					break;
				case 1:
					break;
				case 0:
					break;
			}
		}

		private void ShowLastVisitedPages2()
		{
			var stackPanel = new StackPanel {Orientation = Orientation.Horizontal};
			var stack = NavigationHandler.Pages;

			if (stack.Count <= 0) return;

			for (var index = 0; index < stack.Count - 1; index++)
			{
				var page = stack[index];
				var textBlock = GetTextBlock(page);
				stackPanel.Children.Add(textBlock);

				var delimiterBox = GetDelimiterBlock();
				stackPanel.Children.Add(delimiterBox);
			}

			var lastTextBox = GetTextBlock(stack.Last());
			stackPanel.Children.Add(lastTextBox);

			CommandBar.Content = stackPanel;

			Button GetTextBlock(string text)
			{
				var button = new Button
				{
					Content = text,
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Background = new RevealBackgroundBrush(),
					BorderBrush = new RevealBorderBrush(),
					FontStyle = FontStyle.Italic
				};

				button.Click += Btn_Click;

				return button;
			}

			TextBlock GetDelimiterBlock()
			{
				return new TextBlock
				{
					Text = ">",
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Margin = new Thickness(2, 0, 2, 0)
				};
			}
		}

		private void Btn_Click([NotNull] object sender, RoutedEventArgs e)
		{
			var id = (string) ((Button) sender).Content;
			Debug.WriteLine($"Page Clicked: {id}");

			NavigateToPage(id);
		}

		private void NavigateToPage([NotNull] string id)
		{
			if (Page.Id.Equals(id)) return;

			if (new ContentPageProvider().ContainsContentPage(id))
				Frame.Navigate(typeof(PageViewer), id);
			else
				Frame.Navigate(typeof(NewPage), id);
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Edit Page");
			Frame.Navigate(typeof(PageEditor), contentPageId);
		}

		private async void Revision_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine("Show Revision");
			var wikiFile = await MarkdownWikiFile.MarkdownWikiFile.createMarkdownWikiFileAsync();
			wikiFile.saveAllPagesAsync();
		}

		/// <summary>
		/// A Headeritem in the toc was invoked.
		/// Scrolls to the header in the page.
		/// </summary>
		/// <param name="sender">The pressed header</param>
		/// <param name="args"></param>
		private async void TreeView_ItemInvoked(TreeView sender, [NotNull] TreeViewItemInvokedEventArgs args)
		{
			var node = (MyTreeViewNode) args.InvokedItem;
			var header = (string) node.Content;
			var headerId = node.Tag;

			var scrollTo = $"document.getElementById(\"{headerId}\").scrollIntoView();";

			await Webview.InvokeScriptAsync("eval", new[] {scrollTo});
		}

		private void Print_Click(object sender, RoutedEventArgs e)
		{
			// TODO Print Page
			Debug.WriteLine("Print Page");
		}

		private async void Top_Click(object sender, RoutedEventArgs e)
		{
			await Webview.InvokeScriptAsync("eval", new[] {@"window.scrollTo(0,0);"});
		}

		private async void Pdf_Click(object sender, RoutedEventArgs e)
		{
			//Debug.WriteLine("Print");
			await FileSystemAccess.SaveToFolder();
		}

		private void Favorite_Click(object sender, RoutedEventArgs e)
		{
			Page.IsFavorite = true;
			new ContentPageProvider().UpdateContentPage(Page);
		}

		private void Fullscreen_Click(object sender, RoutedEventArgs e)
		{
			var view = ApplicationView.GetForCurrentView();
			if (view.IsFullScreenMode)
			{
				view.ExitFullScreenMode();
				RightMenu.Visibility = Visibility.Visible;
				RightMenu.MinWidth = 300;
			}
			else
			{
				view.TryEnterFullScreenMode();
				if (view.IsFullScreenMode)
				{
					RightMenu.Visibility = Visibility.Collapsed;
					RightMenu.MinWidth = 0;
				}
			}
		}

		private void Toggle_Click(object sender, RoutedEventArgs e)
		{
			switch (RightMenu.Visibility)
			{
				case Visibility.Visible:
					RightMenu.Visibility = Visibility.Collapsed;
					RightMenu.MinWidth = 0;
					break;
				case Visibility.Collapsed:
					RightMenu.Visibility = Visibility.Visible;
					RightMenu.MinWidth = 300;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			// TODO Settings
		}

		private void Search_Click(object sender, RoutedEventArgs e)
		{
			SearchPopup.IsOpen = true;
		}

		private void Webview_OnScriptNotify(object sender, NotifyEventArgs e)
		{
			Debug.WriteLine("WebView Script");
			if (e.Value == CodeButtonCopy) Debug.WriteLine("Copy Button clicked.");
		}

		private async void Webview_OnLoadCompleted(object sender, NavigationEventArgs e)
		{
			await Webview.InvokeScriptAsync("eval", new[]
			{
				"function codeCopyClickFunction(){" +
				$" window.external.notify('{CodeButtonCopy}');" +
				"}"
			});
		}

		private void MediaManager_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(MediaManager));
		}

		private async void Export_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new ExportDialog(Page.Id);

			var result = await dialog.ShowAsync();
		}
	}
}