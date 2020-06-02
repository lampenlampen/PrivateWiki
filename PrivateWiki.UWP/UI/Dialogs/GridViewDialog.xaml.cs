using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

// Die Elementvorlage "Inhaltsdialogfeld" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UWP.UI.Dialogs
{
	public sealed partial class GridViewDialog : DissmissableDialog
	{
		List<string> list = new List<string>() {"hallo", "huhu", "hihi"};

		public GridViewDialog()
		{
			InitializeComponent();

			Datagrid.ItemsSource = list;
		}

		private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
		{
		}

		private void AddColumn_Click(object sender, RoutedEventArgs e)
		{
			Datagrid.Columns.Add(new DataGridTextColumn());
		}

		private void DataGrid_OnCellEditEnded(object sender, DataGridCellEditEndedEventArgs e)
		{
		}

		private void DataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
		{
		}
	}
}