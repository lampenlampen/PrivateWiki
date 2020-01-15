using JetBrains.Annotations;
using PrivateWiki.Data;
using PrivateWiki.Dialogs;
using System;
using System.Drawing;
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
using Contracts.Storage;
using Models.Pages;
using NLog;
using NodaTime;
using PrivateWiki.Markdig;
using PrivateWiki.Markdig.Extensions.TagExtension;
using PrivateWiki.Pages.ContentPages;
using PrivateWiki.Settings;
using PrivateWiki.Storage;
using PrivateWiki.Utilities;
using PrivateWiki.Utilities.ExtensionFunctions;
using RavinduL.LocalNotifications.Notifications;
using RuntimeComponent;
using StorageBackend.SQLite;
using Page = Windows.UI.Xaml.Controls.Page;
using TreeView = Microsoft.UI.Xaml.Controls.TreeView;
using TreeViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.TreeViewItemInvokedEventArgs;
using PrivateWiki.Controls;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages
{
	/// <summary>
	///     Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class MarkdownPageViewer : ContentPage
	{
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		private readonly string CodeButtonCopy = "codeButtonCopy";

		private IMarkdownPageStorage _storage;

		private MarkdownPage Page { get; set; }

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

				NavigateToPage(wikilink);
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

			Page = await _storage.GetMarkdownPageAsync(pageId);
			Logger.Debug($"Page Some: {pageId}");
			var parser = new Markdig.Markdig();
			var doc = parser.Parse(Page);

			// Show Tags
			var tags = doc.GetTags();

			if (tags != null)
			{
				foreach (var tag in tags)
				{
					var textblock = new TextBlock { Text = tag.Content };

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

			// Show Last Visited Pages
			if (Page != null)
			{
				NavigationHandler.AddPage(Page);
				Logger.Debug($"Last Visited Pages: {NavigationHandler.Pages.Count}");
				ShowLastVisitedPages();
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
			var html = await parser.ToHtmlCustom(doc);
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

		private void ShowLastVisitedPages()
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

			//CommandBar.Content = stackPanel;

			Button GetTextBlock(Path path)
			{
				var button = new Button
				{
					Content = path.Title,
					FontSize = 20,
					VerticalAlignment = VerticalAlignment.Center,
					Background = new RevealBackgroundBrush(),
					BorderBrush = new RevealBorderBrush(),
					FontStyle = FontStyle.Italic
				};

				var tooltip = new ToolTip();
				tooltip.Content = path.FullPath;
				ToolTipService.SetToolTip(button, tooltip);

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
			Logger.Debug($"Page Clicked: {id}");

			NavigateToPage(id);
		}

		private async void NavigateToPage(string link)
		{
			if (Page.Link.Equals(link)) return;

			if (await _storage.ContainsMarkdownPageAsync(link))
				Frame.Navigate(typeof(MarkdownPageViewer), link);
			else
				Frame.Navigate(typeof(NewPage), link);
		}

		private void Edit_Click(object sender, RoutedEventArgs e)
		{
			Logger.Debug("Edit Page");

			if (Page.IsLocked)
			{
				App.Current.manager.Show(new SimpleNotification
				{
					TimeSpan = TimeSpan.FromSeconds(3),
					Text = "Page cannot be edited.",
					Glyph = "\uE1F6",
					VerticalAlignment = VerticalAlignment.Bottom,
					Background = new SolidColorBrush(Color.Red.ToWindowsUiColor())
				});
			}
			else
			{
				Frame.Navigate(typeof(PageEditor), Page.Link);
			}
		}

		private async void Revision_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(HistoryPage), Page.Link);
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

		private async void Top_Click(object sender, RoutedEventArgs e)
		{
			await Webview.InvokeScriptAsync("eval", new[] {"window.scrollTo(0,0);"});
		}

		/// <summary>
		/// This method is called, when the user clicks the "Print"-Button.
		///
		/// Prints the current page to a pdf file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void Pdf_Click(object sender, RoutedEventArgs e)
		{
			// TODO Print PDF
			var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();

			var dialog = new ContentDialog
			{
				Title = resourceLoader.GetString("PrintPDF/Dialog/Title"),
				Content = resourceLoader.GetString("PrintPDF/Dialog/Content"),
				PrimaryButtonText = resourceLoader.GetString("PrintPDF/Dialog/OpenInBrowser"),
				CloseButtonText = resourceLoader.GetString("Close"),
				DefaultButton = ContentDialogButton.Primary
			};

			var result = await dialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				var pageExporter = new PageExporter();
				var file = await pageExporter.ExportPage(Page);

				_ = Launcher.LaunchFileAsync(file);
			}
		}

		private void Fullscreen_Click(object sender, RoutedEventArgs e)
		{
			var view = ApplicationView.GetForCurrentView();
			if (view.IsFullScreenMode) view.ExitFullScreenMode();
			else view.TryEnterFullScreenMode();
		}

		private void Setting_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(SettingsPage));
		}

		private void Search_Click(object sender, RoutedEventArgs e)
		{
			SearchPopup.IsOpen = true;
			SearchPopup.Closed += (sender, e) =>
			{
				var link = SearchPopupContentName.SelectedPageLink;

				if (link != null)
				{
					NavigateToPage(link);
				}
			};
		}

		/// <summary>
		/// Called when the user clicks the "Export"-Button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Export_Click(object sender, RoutedEventArgs e)
		{
			_ = new ExportDialog(Page.Link).ShowAsync();
		}

		/// <summary>
		/// This method is called, if the user clicked the "Import"-Button.
		///
		/// The method should import a markdown page into the database.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Import_Click(object sender, RoutedEventArgs e)
		{
			// TODO Import Dialog

			App.Current.manager.ShowNotImplementedNotification();
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