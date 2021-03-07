using Windows.UI.Xaml;

namespace PrivateWiki.UWP.UI.UI.Controls
{
	public interface IPageViewerCommandBar
	{
		void Edit_Click(object sender, RoutedEventArgs e);

		void Revision_Click(object sender, RoutedEventArgs e);

		void Top_Click(object sender, RoutedEventArgs e);

		/// <summary>
		/// This method is called, when the user clicks the "Print"-Button.
		///
		/// Prints the current page to a pdf file.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Pdf_Click(object sender, RoutedEventArgs e);

		void Fullscreen_Click(object sender, RoutedEventArgs e);

		void Setting_Click(object sender, RoutedEventArgs e);

		void Search_Click(object sender, RoutedEventArgs e);

		/// <summary>
		/// Called when the user clicks the "Export"-Button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Export_Click(object sender, RoutedEventArgs e);

		/// <summary>
		/// This method is called, if the user clicked the "Import"-Button.
		///
		/// The method should import a markdown page into the database.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Import_Click(object sender, RoutedEventArgs e);

		void CommandBar_OnNavigateToPage(object sender, string id);
	}
}