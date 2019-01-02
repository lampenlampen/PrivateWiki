﻿using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class HyperlinkDialog : DissmissableDialog
	{
		public HyperlinkDialog()
		{
			this.InitializeComponent();
		}

		public string Hyperlink { get; private set; }

		private void InsertButton_Click(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
			Hyperlink = HyperlinkTextBox.Text;
		}
	}
}