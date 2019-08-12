using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Benutzersteuerelement" wird unter https://go.microsoft.com/fwlink/?LinkId=234236 dokumentiert.

namespace PrivateWiki.Controls
{
	public sealed partial class NavigationSettingsHeaderItemControl : UserControl
	{
		public NavigationSettingsHeaderItemControl()
		{
			this.InitializeComponent();
		}

		public string HeaderText
		{
			get => (string)GetValue(HeaderTextProperty);
			set => SetValue(HeaderTextProperty, value);
		}

		// Using a DependencyProperty as the backing store for TitleProperty.
		// This enables animation, styling, binding, etc...
		public static readonly DependencyProperty HeaderTextProperty = DependencyProperty.Register("HeaderTextProperty",
			typeof(string), typeof(NavigationSettingsHeaderItemControl), null);

		public event TextChangedEventHandler TextChanged;

		private void Text_OnTextChanged(object sender, TextChangedEventArgs e)
		{
			TextChanged?.Invoke(sender, e);
		}
	}
}
