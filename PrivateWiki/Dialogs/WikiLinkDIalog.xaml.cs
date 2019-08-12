using NodaTime;
using PrivateWiki.Data;
using PrivateWiki.Data.DataAccess;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;

#nullable enable

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class WikiLinkDialog : DissmissableDialog
	{
		public WikiLinkDialog()
		{
			InitializeComponent();

			Pages = new DataAccessImpl().GetPages().Select(p => p.Link).ToList();

			WikiLinkComboBox.ItemsSource = Pages;
		}

		private List<string> Pages { get; }

		public string WikiLink { get; private set; }

		private void InsertHyperlink_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			WikiLink = WikiLinkComboBox.SelectionBoxItem as string;
		}

	}
}