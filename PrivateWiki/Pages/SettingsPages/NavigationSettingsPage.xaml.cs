using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PrivateWiki.Models;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages.SettingsPages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class NavigationSettingsPage : Page
	{
		ObservableCollection<NavigationItem> navigationItems = new ObservableCollection<NavigationItem>();

		public NavigationSettingsPage()
		{
			this.InitializeComponent();
			navigationItems.Add(new DividerItem());
			navigationItems.Add(new HeaderItem());
			navigationItems.Add(new DividerItem());
			navigationItems.Add(new LinkItem());
			navigationItems.Add(new DividerItem());
		}

		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Add_LinkClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Add_HeaderClick(object sender, RoutedEventArgs e)
		{
			throw new NotImplementedException();
		}

		private void Add_DividerClick(object sender, RoutedEventArgs e)
		{
			var listViewItem = new ListViewItem();

			var color = (Color)Application.Current.Resources["SystemAccentColor"];
			var backgroundBrush = new SolidColorBrush(color);

			listViewItem.Background = backgroundBrush;
			listViewItem.MinWidth = 256D;

			var line = new LineGeometry();

			listViewItem.Content = line;

			Listview.Items.Add(line);
		}
	}
}
