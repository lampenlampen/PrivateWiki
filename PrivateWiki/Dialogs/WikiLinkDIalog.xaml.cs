using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Data;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class WikiLinkDialog : DissmissableDialog
	{
		public WikiLinkDialog()
		{
			this.InitializeComponent();

			Pages = new ContentPageProvider().GetAllContentPages().Map(p => p.Id).ToList();

			WikiLinkComboBox.ItemsSource = Pages;
		}

		private List<string> Pages { get; set; }

		public string WikiLink { get; private set; }

		private void InsertHyperlink_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			WikiLink = WikiLinkComboBox.SelectionBoxItem as string;
		}
	}
}