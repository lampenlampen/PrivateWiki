using Markdig;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class PageEditor : Page
    {
        private string pageId;
        private Data.PageAccess pageAccess = new Data.PageAccess();

        public PageEditor()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            pageId = (string)e.Parameter;

            if (pageId == null) throw new ArgumentNullException("Page id must be nonnull!");

            Preview_WebView.Visibility = Visibility.Collapsed;
            showPageInEditor();
        }

        private void showPageInEditor()
        {
            var markdown = pageAccess.GetPage(pageId);

            PageEditorTextBox.Text = markdown;
        }

        private void Preview_Click(object sender, RoutedEventArgs e)
        {
            Preview_WebView.Visibility = Visibility.Visible;
            var markdown = PageEditorTextBox.Text;

            var html = Markdown.ToHtml(markdown);

            Preview_WebView.NavigateToString(html);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            var markdown = PageEditorTextBox.Text;

            pageAccess.UpdatePage("1", markdown);
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
            
        }
    }
}
