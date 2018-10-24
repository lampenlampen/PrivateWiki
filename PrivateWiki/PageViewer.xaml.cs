using Windows.UI.Xaml;
using System;
using PrivateWiki.Data;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using Microsoft.UI.Xaml.Media;
using PrivateWiki.Parser.Markdig;
using PrivateWiki.ParserRenderer.Markdig;
using StorageProvider;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageViewer : Page
    {
        private string contentPageId { get; set; }

        private ContentPage Page { get; set; }

        private string[] SearchEntries { get; } = {"Andreas", "Laura", "Felix", "Lisa", "Anton", "Markus"};

        public PageViewer()
        {
            InitializeComponent();
        }

        private void WebViewNavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            // TODO Interwiki Urls

            var uri = args.Uri;

            // Preview Button Clicked; Do nothing
            if (uri == null || String.IsNullOrEmpty(uri.AbsoluteUri)) return;

            // WikiLink
            if (uri.AbsoluteUri.StartsWith("about::"))
            {
                var wikiLink = uri.AbsoluteUri.Substring(7);
                Debug.WriteLine($"WikiLink: {wikiLink}");
                args.Cancel = true;

                NavigateToPage(wikiLink);
            }

            // Local Link in Document
            if (uri.AbsoluteUri.StartsWith("about:"))
            {
                Debug.WriteLine($"Local Link: {uri.AbsoluteUri}");
                return;
            }

            // Normal Link
            Debug.WriteLine($"Link: {uri.AbsoluteUri}");
            var success = Windows.System.Launcher.LaunchUriAsync(uri);
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

        private void ShowContentPage()
        {
            var provider = new ContentPageProvider();

            if (!provider.ContainsContentPage(contentPageId))
            {
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    return;
                }
            }

            Page = provider.GetContentPage(contentPageId);
            Debug.WriteLine($"Page Some: {contentPageId}");
            var parser = new MarkdigParser();

            // Show Page Title
            //PageTitle.Text = Page.Id;

            // Show Last Visited Pages
            NavigationHandler.AddPage(Page);
            Debug.WriteLine($"Last Visited Pages: {NavigationHandler.Pages.Count}");
            ShowLastVisitedPages();

            // Show TOC
            var doc = parser.Parse(Page);
            var toc = new HeadersParser().ParseHeaders(doc);
            TreeView.RootNodes.Add(toc);

            // Show Page
            var html = parser.ToHtmlString(Page);
            Webview.NavigateToString(html);

            // Show Tags
            if (Page.Tags != null)
            {
                foreach (var tag in Page.Tags)
                {
                    ListView.Items.Add(tag.Name);
                }
            }
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

            if (stack.Count <= 0)
            {
                return;
            }

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
                    Background = new Windows.UI.Xaml.Media.RevealBackgroundBrush(),
                    BorderBrush = new Windows.UI.Xaml.Media.RevealBorderBrush(),
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
            {
                Frame.Navigate(typeof(PageViewer), id);
            }
            else
            {
                Frame.Navigate(typeof(NewPage), id);
            }
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Edit Page");
            this.Frame.Navigate(typeof(PageEditor), contentPageId);
        }

        private void Revision_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Show Revision");
        }

        private async void TreeView_ItemInvoked(TreeView sender,
            [NotNull] TreeViewItemInvokedEventArgs args)
        {
            var headerId = (string) ((TreeViewNode) args.InvokedItem).Content;
            Debug.WriteLine($"Header Clicked: {headerId}");

            var scrollTo = $"document.getElementById(\"{headerId}\").scrollIntoView();";

            await Webview.InvokeScriptAsync("eval", new[] {scrollTo});

            Debug.WriteLine("Scrolled");
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

        /*
        private string GenerateHtml(string markdown)
        {
            var htmlTemplate1 = @"<!DOCTYPE html>
<html lang = ""de"">
<head> 
<style> 
h1 {
    border-bottom-color: rgb(162, 169, 177);
    border-bottom-style: solid;
    border-bottom-width: 1px;
}
h2 {
    border-bottom-color: rgb(162, 169, 177);
    border-bottom-style: solid;
    border-bottom-width: 1px;
}

code {
    background-color: rgb(248, 248, 248);
    display: block;
    line-height: 18.11px;
    padding-bottom: 13.93px;
    padding-left: 13.93px;
    padding-right: 13.93px;
    padding-top: 13.93px;
}
</style>
</head>
<body>
";

            var htmlTemplate2 = @"
</body>
</html>";

            return $"{htmlTemplate1}{markdown}{htmlTemplate2}";
        }


        
        private Render.MarkupRenderer CreateRenderer(Document doc)
        {
            var renderer = new Render.MarkupRenderer(doc, null, null, null)
            {
                Background = Background,
                //BorderBrush = BorderBrush,
                //BorderThickness = BorderThickness,
                CharacterSpacing = CharacterSpacing,
                FontFamily = FontFamily,
                FontSize = FontSize,
                FontStretch = FontStretch,
                FontStyle = FontStyle,
                FontWeight = FontWeight,
                Foreground = Foreground,
                IsTextSelectionEnabled = true,
                Padding = Padding,
                //CodeBackground = new SolidColorBrush(convertHexToColor("#FFF6F8FA")),
                CodeBackground = new SolidColorBrush(Color.FromArgb(250, 246, 248, 250)),
                CodeBorderBrush = new SolidColorBrush(Color.FromArgb(250, 190, 190, 190)),
                CodeBorderThickness = new Thickness(1, 1, 1, 1),
                //InlineCodeBorderThickness = InlineCodeBorderThickness,
                //InlineCodeBackground = InlineCodeBackground,
                //InlineCodeBorderBrush = InlineCodeBorderBrush,
                //InlineCodePadding = InlineCodePadding,
                //InlineCodeFontFamily = InlineCodeFontFamily,
                //CodeForeground = CodeForeground,
                CodeFontFamily = new FontFamily("Consolas"),
                CodePadding = new Thickness(20, 15, 20, 15),
                CodeMargin = new Thickness(0, 15, 0, 15),
                //EmojiFontFamily = new FontFamily("Segoe UI Emoji"),
                Header1FontSize = 50,
                Header1FontWeight = FontWeights.Medium,
                Header1Margin = new Thickness(0, 15, 0, 15),
                Header1Foreground = new SolidColorBrush(Colors.Black),
                Header2FontSize = 40,
                Header2FontWeight = FontWeights.Medium,
                Header2Margin = new Thickness(0, 15, 0, 15),
                Header2Foreground = new SolidColorBrush(Colors.Black),
                Header3FontSize = 35,
                Header3FontWeight = FontWeights.Normal,
                Header3Margin = new Thickness(0, 10, 0, 10),
                Header3Foreground = new SolidColorBrush(Colors.Black),
                Header4FontSize = 25,
                Header4FontWeight = FontWeights.Normal,
                Header4Margin = new Thickness(0, 10, 0, 10),
                Header4Foreground = new SolidColorBrush(Colors.Black),
                Header5FontSize = 20,
                Header5FontWeight = FontWeights.Normal,
                Header5Margin = new Thickness(0, 10, 0, 5),
                Header5Foreground = new SolidColorBrush(Colors.Black),
                HorizontalRuleBrush = new SolidColorBrush(Color.FromArgb(250, 190, 190, 190)),
                HorizontalRuleMargin = new Thickness(0, 20, 0, 20),
                HorizontalRuleThickness = 2,
                //ListMargin = ListMargin,
                //ListGutterWidth = ListGutterWidth,
                //ListBulletSpacing = ListBulletSpacing,
                ParagraphMargin = new Thickness(0, 5, 0, 5),
                QuoteBackground = new SolidColorBrush(Color.FromArgb(10, 0, 0, 0)),
                QuoteBorderBrush = new SolidColorBrush(Color.FromArgb(250, 190, 190, 190)),
                QuoteBorderThickness = new Thickness(3, 0, 0, 0),
                QuoteForeground = new SolidColorBrush(Color.FromArgb(255, 110, 116, 124)),
                QuoteMargin = new Thickness(0, 15, 0, 15),
                QuotePadding = new Thickness(15, 10, 15, 10),
                //TableBorderBrush = TableBorderBrush,
                //TableBorderThickness = TableBorderThickness,
                //TableCellPadding = TableCellPadding,
                //TableMargin = TableMargin,
                TextWrapping = TextWrapping.WrapWholeWords,
                //LinkForeground = LinkForeground,
                //ImageStretch = ImageStretch,
                //ImageMaxHeight = ImageMaxHeight,
                //ImageMaxWidth = ImageMaxWidth,
                //WrapCodeBlock = WrapCodeBlock
            };

            return renderer;
        }
        */
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
    }
}