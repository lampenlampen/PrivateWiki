using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using NodaTime;
using PrivateWiki.StorageBackend;
using PrivateWiki.StorageBackend.SQLite;

#nullable enable

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UI.Dialogs
{
	public sealed partial class WikiLinkDialog : DissmissableDialog
	{
		public WikiLinkDialog()
		{
			InitializeComponent();
			Initialize();
		}

		private async void Initialize()
		{
			Pages = (await new SqLiteBackend(new SqLiteStorage("test"), SystemClock.Instance).GetAllPagesAsync()).Select(p => p.Link).ToList();
			WikiLinkComboBox.ItemsSource = Pages;
		}

		private List<string> Pages { get; set; }

		public string WikiLink { get; private set; }

		private void InsertHyperlink_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			WikiLink = (string) WikiLinkComboBox.SelectionBoxItem;
		}
	}
}