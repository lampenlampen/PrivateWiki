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
using PrivateWiki.Pages.SettingsPages;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		public SettingsPage()
		{
			this.InitializeComponent();

			var generalItem = new SettingsItem {Item = "General", Header = SettingsHeader.Site};
			var navigationItem = new SettingsItem { Item = "Navigation", Header = SettingsHeader.Site };
			var pagesItem = new SettingsItem { Item = "Pages", Header = SettingsHeader.Site };
			var themeItem = new SettingsItem { Item = "Theme", Header = SettingsHeader.Site };
			var renderingItem = new SettingsItem { Item = "Rendering", Header = SettingsHeader.Modules };
			var storageItem = new SettingsItem { Item = "Storage", Header = SettingsHeader.Modules };
			var developerToolsItem = new SettingsItem { Item = "Developer Tools", Header = SettingsHeader.System };

			List<SettingsItem> settingsItems = new List<SettingsItem>();
			settingsItems.Add(generalItem);
			settingsItems.Add(navigationItem);
			settingsItems.Add(pagesItem);
			settingsItems.Add(themeItem);
			settingsItems.Add(renderingItem);
			settingsItems.Add(storageItem);
			settingsItems.Add(developerToolsItem);

			var groupeditems = settingsItems.GroupBy(s => s.Header);

			cvs.Source = groupeditems;
		}

		private void SettingsMenuClick(object sender, ItemClickEventArgs args)
		{
			var clickedItem = (SettingsItem) args.ClickedItem;

			switch (clickedItem.Item)
			{
				case "General":
					break;
				case "Navigation":
					SettingsContentFrame.Navigate(typeof(NavigationSettingsPage));
					break;
				case "Pages":
					break;
				case "Theme":
					break;
				case "Rendering":
					break;
				case "Storage":
					break;
				case "Developer Tools":
					break;
				default: throw new NotSupportedException("Settings Item not supported!");
			}
		}
	}

	public class SettingsItem
	{
		public string Item { get; set; }
		public SettingsHeader Header { get; set; }

		public String HeaderString => Header.ToString();
	}
}
