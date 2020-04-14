using System;
using System.Collections.Generic;
using System.Linq;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using PrivateWiki.UI.Pages.SettingsPages;
using muxc = Microsoft.UI.Xaml.Controls;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UI.Pages
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
			("pages", typeof(PagesSettingsPage)),
			("assets", typeof(AssetsSettingsPage)),
			("rendering", typeof(RenderingSettingsPage)),
			("storage", typeof(StorageSettingsPage)),
			("developertools", typeof(DeveloperSettingsPage)),
			("sync", typeof(SyncSettingsPage))
		};

		public SettingsPage()
		{
			this.InitializeComponent();
		}

		private void NavView_Loaded(object sender, RoutedEventArgs e)
		{
			// Add keyboard accelerators for backwards navigation.
			var goBack = new KeyboardAccelerator {Key = VirtualKey.GoBack};
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

		private void NavView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
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

		private void NavView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}
	}
}