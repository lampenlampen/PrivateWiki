using PrivateWiki.Pages.SettingsPages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using NavigationView = Microsoft.UI.Xaml.Controls.NavigationView;
using NavigationViewBackRequestedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewBackRequestedEventArgs;
using NavigationViewItemInvokedEventArgs = Microsoft.UI.Xaml.Controls.NavigationViewItemInvokedEventArgs;
using muxc = Microsoft.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.Pages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class SettingsPage : Page
	{
		// List of ValueTuple holding the Navigation Tag and the relative Navigation Page
		private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
		{
			("navigation", typeof(NavigationSettingsPage)),
			("rendering", typeof(RenderingSettingsPage)),
			("developertools", typeof(DeveloperSettingsPage)),
		};

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

		private void NavView_Loaded(object sender, RoutedEventArgs e)
		{
			// Add keyboard accelerators for backwards navigation.
			var goBack = new KeyboardAccelerator { Key = VirtualKey.GoBack };
			goBack.Invoked += BackInvoked;
			this.KeyboardAccelerators.Add(goBack);

			// ALT routes here
			var altLeft = new KeyboardAccelerator
			{
				Key = VirtualKey.Left,
				Modifiers = VirtualKeyModifiers.Menu
			};
			altLeft.Invoked += BackInvoked;
			this.KeyboardAccelerators.Add(altLeft);
		}

		private void BackInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
		{
			On_BackRequested();
			args.Handled = true;
		}

		private bool On_BackRequested()
		{
			if (!SettingsContentFrame.CanGoBack)
				return false;

			// Don't go back if the nav pane is overlayed.
			if (NavView.IsPaneOpen &&
			    (NavView.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
			     NavView.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
				return false;

			SettingsContentFrame.GoBack();
			return true;
		}

		private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
		{
			if (args.IsSettingsInvoked == true)
			{
				NavView_Navigate("settings", args.RecommendedNavigationTransitionInfo);
			}
			else if (args.InvokedItemContainer != null)
			{
				var navItemTag = args.InvokedItemContainer.Tag.ToString();
				NavView_Navigate(navItemTag, args.RecommendedNavigationTransitionInfo);
			}
		}

		private void NavView_Navigate(string navItemTag, NavigationTransitionInfo transitionInfo)
		{
			Type _page = null;
			if (navItemTag == "settings")
			{
				_page = typeof(SettingsPage);
			}
			else
			{
				var item = _pages.FirstOrDefault(p => p.Tag.Equals(navItemTag));
				_page = item.Page;
			}
			// Get the page type before navigation so you can prevent duplicate
			// entries in the backstack.
			var preNavPageType = SettingsContentFrame.CurrentSourcePageType;

			// Only navigate if the selected page isn't currently loaded.
			if (!(_page is null) && !Type.Equals(preNavPageType, _page))
			{
				SettingsContentFrame.Navigate(_page, null, transitionInfo);
			}
		}

		private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
		{
			if (Frame.CanGoBack) Frame.GoBack();
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
