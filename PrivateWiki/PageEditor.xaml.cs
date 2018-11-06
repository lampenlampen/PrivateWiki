using PrivateWiki.Data;
using StorageProvider;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JetBrains.Annotations;
using LanguageExt;
using LanguageExt.SomeHelp;
using PrivateWiki.Parser.Markdig;
using PrivateWiki.ParserRenderer.Markdig;
using static LanguageExt.Prelude;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageEditor : Page
    {
        [NotNull] private ContentPage Page { get; set; }
        private bool NewPage { get; set; } = false;

        public PageEditor()
        {
            this.InitializeComponent();
        }

        private void PreviewWebviewNavigationStartedAsync(WebView sender, [NotNull] WebViewNavigationStartingEventArgs args)
        {
            var uri = args.Uri;

            // Preview Button Clicked; Do nothing
            if (uri == null || string.IsNullOrEmpty(uri.AbsoluteUri)) return;

            // WikiLink
            if (uri.AbsoluteUri.StartsWith("about::"))
            {
                Debug.WriteLine($"WikiLink: {uri.AbsoluteUri}");
                args.Cancel = true;
                return;
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

            var pageId = (string) e.Parameter;
            Debug.WriteLine($"Id: {pageId}");
            if (pageId == null) throw new ArgumentNullException("Page id must be nonnull!");

            var pageProvider = new ContentPageProvider();

            if (pageProvider.ContainsContentPage(pageId))
            {
                Page = pageProvider.GetContentPage(pageId);
            }
            else
            {
                NewPage = true;
                Page = ContentPage.Create(pageId);
            }

            ShowPageInEditor();
            if (NewPage)
            {
                RemoveNewPageFromBackStack();
            }
        }

        private void ShowPageInEditor()
        {
            PageEditorTextBox.Text = Page.Content;
        }

        private void RemoveNewPageFromBackStack()
        {
            if (Frame.CanGoBack)
            {
                var backstack = Frame.BackStack;
                var lastEntry = backstack[Frame.BackStackDepth - 1];
                if (lastEntry.SourcePageType == typeof(NewPage))
                {
                    Debug.WriteLine("Remove NewPage from BackStack");
                    backstack.Remove(lastEntry);
                }
            }
        }

        private void RemoveEditorPageFromBackStack()
        {
            if (Frame.CanGoBack)
            {
                var backstack = Frame.BackStack;
                var lastEntry = backstack[Frame.BackStackDepth - 1];
                if (lastEntry.SourcePageType == typeof(PageEditor))
                {
                    Debug.WriteLine("Remove EditorPage from BackStack");
                    backstack.Remove(lastEntry);
                }
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Page.Content = PageEditorTextBox.Text;

            if (NewPage)
            {
                // TODO Error while Inserting Page
                new ContentPageProvider().InsertContentPage(Page);

                Frame.Navigate(typeof(PageViewer), Page.Id);
                RemoveEditorPageFromBackStack();
            }
            else
            {
                // TODO Error while Updating Page
                new ContentPageProvider().UpdateContentPage(Page);

                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("GoBack");
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ContentDialog
            {
                Title = $"Delete Page {Page.Id}",
                Content =
                    "Delete this page permanently. After this action there is no way of restoring the current state.",
                CloseButtonText = "Cancel",
                PrimaryButtonText = "Delete Page",
                DefaultButton = ContentDialogButton.Close
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Delete the page.
                Debug.WriteLine("Delete");
                new ContentPageProvider().DeleteContentPage(Page);

                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                    Frame.GoBack();
                }
            }
            else
            {
                // Cancel Delete Action
            }
        }

        private async void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Pivot.SelectedIndex == 1)
            {
                var htmlFileName = "index_preview.html";
                var parser = new MarkdigParser();
                var html = parser.ToHtmlString(Page);
                var localFolder = ApplicationData.Current.LocalFolder;
                var mediaFolder = await localFolder.GetFolderAsync("media");
                var file = await mediaFolder.CreateFileAsync(htmlFileName, CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, html);

                Preview_WebView.Navigate(new Uri($"ms-appdata:///local/media/{htmlFileName}"));
            }

            if (Pivot.SelectedIndex == 2)
            {
                foreach (var tag in Page.Tags)
                {
                    ListView.Items.Add(tag.Name);
                }
            }
        }

        private void AddTag_Click(object sender, RoutedEventArgs e)
        {
            var tagName = AddTagBox.Text;
            var tag = new Tag
            {
                Name = tagName
            };

            // TODO Save Tags to DB
            //Page.Tags.Add(tag);
            ListView.Items.Add(tagName);
        }

        private void VS_Code_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ExternalEditor), Page.Id);
        }
    }
}