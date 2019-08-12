using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Dialogs
{
	public sealed partial class GridViewDialog : DissmissableDialog
	{
		public GridViewDialog()
		{
			InitializeComponent();

			var list = new[] { "hallo", "huhu", "hihi" };

			DataGrid.ItemsSource = list;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void AddColumn_Click(object sender, RoutedEventArgs e)
		{
			DataGrid.Columns.Add(new DataGridTextColumn());
		}

		private void DataGrid_OnCellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
		{
		}

		private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
		}
	}
}