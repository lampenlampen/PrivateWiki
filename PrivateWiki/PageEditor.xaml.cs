using PrivateWiki.Data;
using StorageProvider;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageEditor : Page
    {
        private ContentPage Page { get; set; }

        public PageEditor()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var pageId = (string)e.Parameter;
            Debug.WriteLine($"Id: {pageId}");
            if (pageId == null) throw new ArgumentNullException("Page id must be nonnull!");

            Page = new ContentPageProvider().GetContentPage(pageId);

            Preview_WebView.Visibility = Visibility.Collapsed;

            ShowPageInEditor();
        }

        private void ShowPageInEditor()
        {
            PageEditorTextBox.Text = Page.Content;
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            Preview_WebView.Visibility = Visibility.Visible;
            var markdown = PageEditorTextBox.Text;

            var html = new Parser.MarkdigParser().ToHtmlString(Page);

            Preview_WebView.NavigateToString(html);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = PageEditorTextBox.Text;

            new ContentPageProvider().UpdateContentPage(Page);
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("GoBack");
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
