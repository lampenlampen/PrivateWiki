using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UI.Dialogs
{
	public sealed partial class EditorImagePickerDialog : ContentDialog
	{
		public EditorImagePickerDialog()
		{
			this.InitializeComponent();
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}
	}
}