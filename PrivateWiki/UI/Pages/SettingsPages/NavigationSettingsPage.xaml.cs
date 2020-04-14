using System;
using System.Collections.ObjectModel;
using System.IO;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrivateWiki.Models;
using PrivateWiki.UI.Controls;
using PrivateWiki.Utilities;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UI.Pages.SettingsPages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class NavigationSettingsPage : Page
	{
		private FullyObservableCollection<NavigationItem> navigationItems =
			new FullyObservableCollection<NavigationItem>();

		public NavigationSettingsPage()
		{
			InitializeComponent();
			LoadNavigationItems();
		}

		private async void StoreNavigationItems(Collection<NavigationItem> items)
		{
			var roamingFolder = ApplicationData.Current.RoamingFolder;
			var settingsFolder = await roamingFolder.CreateFolderAsync("settings", CreationCollisionOption.OpenIfExists);
			var navSettingsFile = await settingsFolder.CreateFileAsync("nav_settings.json", CreationCollisionOption.ReplaceExisting);
			using var stream = await navSettingsFile.OpenStreamForWriteAsync();

			var writer = new JsonTextWriter(new StreamWriter(stream));

			writer.Formatting = Formatting.Indented;
			writer.WriteStartObject();
			writer.WritePropertyName("version");
			writer.WriteValue("0.1");
			writer.WritePropertyName("navigation_items");
			writer.WriteStartArray();

			var navigationItemJsonSerializer = new NavigationItemJsonSerializer(writer);

			foreach (var item in navigationItems)
			{
				navigationItemJsonSerializer.WriteJson(item);
			}

			writer.WriteEndArray();
			writer.WriteEndObject();

			writer.Flush();
		}

		private async void LoadNavigationItems()
		{
			var roamingFolder = ApplicationData.Current.RoamingFolder;
			var settingsFolder = await roamingFolder.CreateFolderAsync("settings", CreationCollisionOption.OpenIfExists);

			// Bug FileNotFound
			var navSettingsFile = await settingsFolder.GetFileAsync("nav_settings.json");
			var json = await FileIO.ReadTextAsync(navSettingsFile);

			JObject rss = JObject.Parse(json);

			var version = (string) rss["version"];

			JArray items = (JArray) rss["navigation_items"];
			var count = items.Count;

			foreach (var item in items)
			{
				var type = (string) item["type"];

				switch (type)
				{
					case "divider":
						var dividerItem = new DividerItem();
						navigationItems.Add(dividerItem);
						break;
					case "header":
						var headerItem = new HeaderItem
						{
							Label = (string) item["label"]
						};
						navigationItems.Add(headerItem);
						break;
					case "link":
						var linkItem = new LinkItem
						{
							Label = (string) item["label"],
							PageId = Guid.Parse((string) item["link_id"])
						};
						navigationItems.Add(linkItem);
						break;
				}
			}
		}

		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			StoreNavigationItems(navigationItems);
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			navigationItems.Clear();
			LoadNavigationItems();
		}

		private void Add_LinkClick(object sender, RoutedEventArgs e)
		{
			var link = new LinkItem {Label = "Link", Id = Guid.NewGuid()};
			navigationItems.Add(link);
		}

		private void Add_HeaderClick(object sender, RoutedEventArgs e)
		{
			var header = new HeaderItem {Label = "Header", Id = Guid.NewGuid()};
			navigationItems.Add(header);
		}

		private void Add_DividerClick(object sender, RoutedEventArgs e)
		{
			var divider = new DividerItem {Id = Guid.NewGuid()};
			navigationItems.Add(divider);
		}

		private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			NavigationSettingsItemContent.Children.Clear();

			var item = (NavigationItem) ((ListView) sender).SelectedItem;

			switch (item)
			{
				case HeaderItem headerItem:
				{
					var headerControl = new NavigationSettingsHeaderItemControl();
					headerControl.initControl(headerItem.Label);

					headerControl.TextChanged += (o, args) => { headerItem.Label = ((NavigationSettingsHeaderItemControl) o).HeaderText; };
					headerControl.DeleteHeader += (o, args) =>
					{
						var item = (NavigationItem) Listview.SelectedItem;
						navigationItems.Remove(item);
					};

					NavigationSettingsItemContent.Children.Add(headerControl);
					break;
				}
				case DividerItem dividerItem:
				{
					var dividerControl = new NavigationSettingsDividerItemControl();

					dividerControl.DeleteDivider += (o, args) =>
					{
						var item = (NavigationItem) Listview.SelectedItem;
						navigationItems.Remove(item);
					};

					NavigationSettingsItemContent.Children.Add(dividerControl);
					break;
				}
				case LinkItem linkItem:
				{
					var linkControl = new NavigationSettingsLinkItemControl();
					linkControl.InitControl(linkItem.Label, linkItem.PageId);


					linkControl.DeleteLink += DeleteNavigationItem;
					linkControl.LabelChanged += (o, args) => { linkItem.Label = ((NavigationSettingsLinkItemControl) o).Label; };
					linkControl.LinkSelected += id => linkItem.PageId = id;

					NavigationSettingsItemContent.Children.Add(linkControl);
					break;
				}
				default:
					break;
			}
		}

		private void DeleteNavigationItem(object o, RoutedEventArgs args)
		{
			var item = (NavigationItem) Listview.SelectedItem;
			navigationItems.Remove(item);
		}
	}
}