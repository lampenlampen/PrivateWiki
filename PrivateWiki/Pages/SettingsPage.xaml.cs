using PrivateWiki.Pages.SettingsPages;
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

			var generalItem = new SettingsItem(SettingItems.General, SettingHeaders.Site);
			var navigationItem = new SettingsItem(SettingItems.Navigation, SettingHeaders.Site);
			var pagesItem = new SettingsItem(SettingItems.Pages, SettingHeaders.Site);
			var themeItem = new SettingsItem(SettingItems.Theme, SettingHeaders.Site);
			var renderingItem = new SettingsItem(SettingItems.Rendering, SettingHeaders.Modules);
			var storageItem = new SettingsItem(SettingItems.Storage, SettingHeaders.Modules);
			var developerToolsItem = new SettingsItem(SettingItems.DeveloperTools, SettingHeaders.System) { ItemString = "Developer Tools" };

			List<SettingsItem> settingsItems = new List<SettingsItem>
			{
				generalItem,
				navigationItem,
				pagesItem,
				themeItem,
				renderingItem,
				storageItem,
				developerToolsItem
			};

			var groupedItems = settingsItems.GroupBy(s => s.HeaderString);

			cvs.Source = groupedItems;
		}

		private void SettingsMenuClick(object sender, ItemClickEventArgs args)
		{
			var clickedItem = (SettingsItem)args.ClickedItem;

			switch (clickedItem.Item)
			{
				case SettingItems.General:
					break;
				case SettingItems.Navigation:
					SettingsContentFrame.Navigate(typeof(NavigationSettingsPage));
					break;
				case SettingItems.Pages:
					break;
				case SettingItems.Theme:
					break;
				case SettingItems.Rendering:
					SettingsContentFrame.Navigate(typeof(RenderingSettingsPage));
					break;
				case SettingItems.Storage:
					break;
				case SettingItems.DeveloperTools:
					SettingsContentFrame.Navigate(typeof(DeveloperSettingsPage));
					break;
			}
		}
	}

	public class SettingsItem
	{
		public SettingItems Item { get; set; }

		public string ItemString { get; set; }
		public SettingHeaders Header { get; set; }

		public String HeaderString { get; set; }

		public SettingsItem(SettingItems item, SettingHeaders header)
		{
			Item = item;
			Header = header;
			ItemString = item.ToString();
			HeaderString = header.ToString();
		}
	}

	public enum SettingItems
	{
		General,
		Navigation,
		Rendering,
		Pages,
		Theme,
		Storage,
		DeveloperTools
	}

	public enum SettingHeaders
	{
		Site = 0,
		Modules = 1,
		System = 2
	}
}
