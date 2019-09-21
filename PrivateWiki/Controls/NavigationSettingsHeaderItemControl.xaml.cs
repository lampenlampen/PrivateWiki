using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace PrivateWiki.Controls
{
	public sealed partial class NavigationSettingsHeaderItemControl : UserControl
	{
		public Guid Id { get; set; }

		public string HeaderText => Text.Text;

		public NavigationSettingsHeaderItemControl()
		{
			this.InitializeComponent();
		}

		public void initControl(string label)
		{
			Text.Text = label;
		}

		public event TextChangedEventHandler TextChanged;

		private void Text_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			TextChanged?.Invoke(this, e);
		}

		public event RoutedEventHandler DeleteHeader;

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			DeleteHeader?.Invoke(this, e);
		}
	}
}
