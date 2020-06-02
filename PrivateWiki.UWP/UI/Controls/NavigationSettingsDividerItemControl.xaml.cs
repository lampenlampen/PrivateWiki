using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.UWP.UI.Controls
{
	public sealed partial class NavigationSettingsDividerItemControl : UserControl
	{
		public NavigationSettingsDividerItemControl()
		{
			this.InitializeComponent();
		}

		public event RoutedEventHandler DeleteDivider;

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DeleteDivider?.Invoke(this, e);
		}
	}
}