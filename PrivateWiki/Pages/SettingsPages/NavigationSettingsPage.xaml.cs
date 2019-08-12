using PrivateWiki.Controls;
using PrivateWiki.Models;
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

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages.SettingsPages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class NavigationSettingsPage : Page
	{
		private ObservableCollection<NavigationItem> navigationItems = new ObservableCollection<NavigationItem>();

		public NavigationSettingsPage()
		{
			this.InitializeComponent();
			navigationItems.Add(new DividerItem());
			navigationItems.Add(new HeaderItem { Text = "Header" });
			navigationItems.Add(new DividerItem());
			navigationItems.Add(new LinkItem() { Text = "Link" });
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
			var link = new LinkItem { Text = "Link" };
			navigationItems.Add(link);
		}

		private void Add_HeaderClick(object sender, RoutedEventArgs e)
		{
			var header = new HeaderItem { Text = "Header" };
			navigationItems.Add(header);
		}

		private void Add_DividerClick(object sender, RoutedEventArgs e)
		{
			var divider = new DividerItem();
			navigationItems.Add(divider);
		}

		private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			NavigationSettingsItemContent.Children.Clear();

			var item = (NavigationItem)((ListView)sender).SelectedItem;

			if (item is HeaderItem headerItem)
			{
				var headerControl = new NavigationSettingsHeaderItemControl();
				headerControl.TextChanged += (o, args) => { headerItem.Text = headerControl.HeaderText; };
				headerControl.DeleteHeader += (o, args) =>
				{
					var sender = o;
				};

				NavigationSettingsItemContent.Children.Add(headerControl);
			}
			else if (item is DividerItem dividerItem)
			{
				var dividerControl = new NavigationSettingsDividerItemControl();
				NavigationSettingsItemContent.Children.Add(dividerControl);
			}
			else if (item is LinkItem linkItem)
			{

			}
			else
			{
				throw new ArgumentException();
			}
		}


	}
}
