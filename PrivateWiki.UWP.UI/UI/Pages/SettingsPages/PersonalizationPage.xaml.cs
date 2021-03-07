using Windows.UI.Xaml.Controls;
using PrivateWiki.ViewModels.Settings;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PersonalizationPage : Page
	{
		public PersonalizationPage()
		{
			this.InitializeComponent();

			PersonalizationCtrl.ViewModel = ServiceLocator.Container.GetInstance<PersonalizationCtrlVM>();
		}
	}
}