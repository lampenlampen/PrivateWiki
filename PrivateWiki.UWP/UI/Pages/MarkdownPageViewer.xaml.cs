using System;
using System.Drawing;
using System.Text;
using Windows.Storage;
using Windows.System;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using NLog;
using NodaTime;
using PrivateWiki.Rendering.Markdown.Markdig;
using PrivateWiki.Rendering.Markdown.Markdig.Extensions.TagExtension;
using PrivateWiki.Settings;
using PrivateWiki.StorageBackend;
using PrivateWiki.StorageBackend.SQLite;
using PrivateWiki.UI.Controls;
using PrivateWiki.UI.Pages.ContentPages;
using PrivateWiki.Utilities.ExtensionFunctions;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UI.Pages
{
	/// <summary>
	///     Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class MarkdownPageViewer : ContentPage
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly string CodeButtonCopy = "codeButtonCopy";

		//private MarkdownPage Page { get; set; }

		private string _uri;

		public MarkdownPageViewer()
		{
			InitializeComponent();
			_storage = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);

			Init();
		}

		private void Init()
		{
			// Acrylic Background
			var handler = new DeveloperSettingsModelHandler();
			var acrylicBackground = handler.LoadDeveloperSettingsModel().IsAcrylicBackgroundEnabled;

			if (acrylicBackground)
			{
				PageViewerGrid.Background = (Brush) Application.Current.Resources["SystemControlAcrylicWindowBrush"];
				Webview.DefaultBackgroundColor = Color.Transparent.ToWindowsUiColor();
			}
			else
			{
				PageViewerGrid.Background = new SolidColorBrush(Color.White.ToWindowsUiColor());
				Webview.DefaultBackgroundColor = Color.White.ToWindowsUiColor();
			}
		}

		private void WebView_OnNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
		{
			var uri = args.Uri;

			// Preview Button Clicked; Do nothing
			if (uri == null || string.IsNullOrEmpty(uri.AbsoluteUri)) return;

			// First navigation to page
			if (uri.AbsoluteUri.Equals(_uri)) return;


			// TODO Refactor Link Handling; Use "link"
			//var link = uri.AbsoluteUri.Substring(_uri.Length);

			// WikiLink
			var splittedLink = uri.AbsoluteUri.Split(':', StringSplitOptions.RemoveEmptyEntries);
			if (splittedLink.Length >= 3)
			{
				var builder = new StringBuilder();
				builder.Append(splittedLink[2]);
				for (var i = 3; i < splittedLink.Length; i++) builder.Append($":{splittedLink[i]}");

				var wikilink = builder.ToString();

				Logger.Debug($"WikiLink: {wikilink}");
				args.Cancel = true;

				NavigateToPage(Page, wikilink);
			}

			// Local Link in Document
			if (uri.AbsoluteUri.StartsWith("about:"))
			{
				Logger.Debug($"Local Link: {uri.AbsoluteUri}");
				return;
			}

			if (uri.AbsoluteUri.StartsWith("ms-local-stream:"))
			{
				Logger.Debug($"Local HtmlFile: {uri.AbsoluteUri}");
				return;
			}

			// Normal Link
			Logger.Debug($"Link: {uri.AbsoluteUri}");
			Launcher.LaunchUriAsync(uri);
			args.Cancel = true;
		}

		private void Webview_OnScriptNotify(object sender, NotifyEventArgs e)
		{
			Logger.Debug($"WebView Script+ {e.Value}");
			if (e.Value == CodeButtonCopy) Logger.Debug("Copy Button clicked.");
			else if (e.Value == "key:strg+e")
			{
				Edit_Click(sender, new RoutedEventArgs());
			}
			else if (e.Value == "key:strg+p")
			{
				Pdf_Click(sender, new RoutedEventArgs());
			}
			else if (e.Value == "key:strg+s")
			{
				Search_Click(sender, new RoutedEventArgs());
			}
		}

		private async void Webview_OnLoadCompleted(object sender, NavigationEventArgs e)
		{
		}

		protected override void OnNavigatedTo([NotNull] NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			var pageId = (string) e.Parameter;
			Logger.Debug($"Id: {pageId}");

			if (pageId == null || pageId.Equals("")) throw new ArgumentNullException(nameof(pageId));

			DisplayPage(pageId);
		}

		private async void DisplayPage(string pageId)
		{
			if (!await _storage.ContainsMarkdownPageAsync(pageId))
				if (Frame.CanGoBack)
				{
					Frame.GoBack();
					return;
				}

			Page = await new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance).GetPageAsync(pageId);
			// Page = await _storage.GetPageAsync(pageId);
			Logger.Debug($"Page Some: {pageId}");
			var parser = new Rendering.Markdown.Markdig.Markdig();
			var doc = parser.Parse(Page.Content);

			// Show Tags
			var tags = doc.GetTags();

			if (tags != null)
			{
				foreach (var tag in tags)
				{
					var textblock = new TextBlock {Text = tag.Content};

					var border = new Border
					{
						CornerRadius = new CornerRadius(12),
						BorderThickness = new Thickness(0),
						Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 224, 224, 224)),
						Margin = new Thickness(5, 5, 5, 5),
						Padding = new Thickness(10, 5, 10, 5),
						Child = textblock
					};

					TagsPanel.Children.Add(border);
				}
			}

			// Show TOC
			// Only possible if CoreRenderModel.IsAutoIdentifierEnabled is true
			var handler = new RenderingModelHandler();
			var model = handler.LoadCoreModel(handler.GetRenderingSettingsContainer());

			if (model.IsAutoIdentifierEnabled)
			{
				try
				{
					var toc = new HeadersParser().ParseHeaders(doc);
					foreach (var header in toc.Children) Treeview.RootNodes.Add(header);
				}
				catch (Exception e)
				{
					Logger.Error(e, "Only one top level header allowed!");
				}
			}

			// Show Page
			var html = await parser.ToHtmlCustomAsync(doc);
			var localFolder = ApplicationData.Current.LocalFolder;
			var mediaFolder = await localFolder.CreateFolderAsync("media", CreationCollisionOption.OpenIfExists);
			var file = await mediaFolder.CreateFileAsync("index.html", CreationCollisionOption.ReplaceExisting);
			await FileIO.WriteTextAsync(file, html);

			var webViewFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(@"Assets\WebView");

			var styleSheetFile = await webViewFolder.GetFileAsync("preferred.css");
			await styleSheetFile.CopyAsync(mediaFolder, styleSheetFile.Name, NameCollisionOption.ReplaceExisting);

			var javascriptFile = await webViewFolder.GetFileAsync("index.js");
			await javascriptFile.CopyAsync(mediaFolder, javascriptFile.Name, NameCollisionOption.ReplaceExisting);

			var uri = Webview.BuildLocalStreamUri("MyTag", "/media/index.html");
			_uri = uri.AbsoluteUri;
			var uriResolver = new MyUriToStreamResolver();
			Webview.NavigateToLocalStreamUri(uri, uriResolver);
		}

		/// <summary>
		/// A Headeritem in the toc was invoked.
		/// Scrolls to the header in the page.
		/// </summary>
		/// <param name="sender">The pressed header</param>
		/// <param name="args"></param>
		private async void TreeView_ItemInvoked(TreeView treeView, TreeViewItemInvokedEventArgs args)
		{
			var node = (MyTreeViewNode) args.InvokedItem;
			var header = (string) node.Content;
			var headerId = node.Tag;
			var scrollTo = $"document.getElementById(\"{headerId}\").scrollIntoView();";

			await Webview.InvokeScriptAsync("eval", new[] {scrollTo});
		}

		public override async void Top_Click(object sender, RoutedEventArgs e)
		{
			await Webview.InvokeScriptAsync("eval", new[] {"window.scrollTo(0,0);"});
		}

		public override void Search_Click(object sender, RoutedEventArgs e)
		{
			SearchPopup.IsOpen = true;
			SearchPopup.Closed += (sender, e) =>
			{
				var link = SearchPopupContentName.SelectedPageLink;

				if (link != null)
				{
					NavigateToPage(Page, link);
				}
			};
		}

		private void Webview_OnContainsFullScreenElementChanged(WebView sender, object args)
		{
			var applicationView = ApplicationView.GetForCurrentView();

			if (sender.ContainsFullScreenElement)
			{
				applicationView.TryEnterFullScreenMode();
			}
			else if (applicationView.IsFullScreenMode)
			{
				applicationView.ExitFullScreenMode();
			}
		}
	}
}