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
using PrivateWiki.Data;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
    public sealed partial class WikiLinkDialog : DissmissableDialog
    {
        private List<string> Pages { get; set; }

        public string WikiLink {get; private set; }

        public WikiLinkDialog()
        {
            this.InitializeComponent();

            Pages = new ContentPageProvider().GetAllContentPages().Map(p => p.Id).ToList();

            WikiLinkComboBox.ItemsSource = Pages;
        }

        private void InsertHyperlink_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            WikiLink = WikiLinkComboBox.SelectionBoxItem as string;
        }


    }
}
