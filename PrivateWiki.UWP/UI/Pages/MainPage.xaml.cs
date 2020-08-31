using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using PrivateWiki.ViewModels;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x407 dokumentiert.

namespace PrivateWiki.UWP.UI.Pages
{
	/// <summary>
	///     Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		public MainPage()
		{
			InitializeComponent();

			// TODO Acrylic
			// ExtendAcrylicIntoTitleBar();

			// Reverse Acrylic Titlebar
			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;
			// Default TitleBar Button Background Color
			ApplicationView.GetForCurrentView().TitleBar.ButtonBackgroundColor =
				new UISettings().GetColorValue(UIColorType.Accent);

			//Testing
			// EditorFrame.Navigate(typeof(PageViewer), "start");

			CreateLabelControl.ViewModel = new CreateNewLabelControlViewModel();
		}

		/// Extend acrylic into the title bar.
		private void ExtendAcrylicIntoTitleBar()
		{
			CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;
			var titleBar = ApplicationView.GetForCurrentView().TitleBar;
			titleBar.ButtonBackgroundColor = Colors.Transparent;
			titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
		}
	}
}