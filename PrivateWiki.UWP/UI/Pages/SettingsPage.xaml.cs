using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;
using JetBrains.Annotations;
using PrivateWiki.Core.Events;
using PrivateWiki.Services.TranslationService;
using PrivateWiki.UWP.UI.Pages.SettingsPages;
using PrivateWiki.UWP.UI.Pages.SettingsPagesOld;
using muxc = Microsoft.UI.Xaml.Controls;

#nullable enable

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace PrivateWiki.UWP.UI.Pages
{
	/// <summary>
	/// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
	/// </summary>
	public sealed partial class SettingsPage : Page, INotifyPropertyChanged
	{
		private readonly IObservable<CultureChangedEventArgs> _cultureChangedEvent;

		public SettingsPageResources Translations { get; }

		// List of ValueTuple holding the Navigation Tag and the relative Navigation Page
		private readonly List<(string Tag, Type Page)> _pages = new List<(string Tag, Type Page)>
		{
			("navigation", typeof(NavigationSettingsPage)),
			("pages", typeof(PagesSettingsPage)),
			("assets", typeof(AssetsSettingsPage)),
			("rendering", typeof(RenderingSettingsPage)),
			("storage", typeof(StorageSettingsPage)),
			("developertools", typeof(DeveloperSettingsPage)),
			("sync", typeof(BackupSyncSettingsPage)),
			("labels", typeof(LabelsSettingsPage)),
			("personalization", typeof(PersonalizationPage)),
		};

		public SettingsPage()
		{
			this.InitializeComponent();

			var container = ServiceLocator.Container;
			TranslationManager translationManager = container.GetInstance<TranslationManager>();
			_cultureChangedEvent = container.GetInstance<IObservable<CultureChangedEventArgs>>();

			Translations = new SettingsPageResources(translationManager);

			_cultureChangedEvent.Subscribe(x => OnPropertyChanged(nameof(Translations)));
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

		private void On_BackRequested()
		{
			if (!SettingsContentFrame.CanGoBack) return;

			// Don't go back if the nav pane is overlayed.
			if (NavView.IsPaneOpen &&
			    (NavView.DisplayMode == muxc.NavigationViewDisplayMode.Compact ||
			     NavView.DisplayMode == muxc.NavigationViewDisplayMode.Minimal))
				return;

			SettingsContentFrame.GoBack();
		}

		private void NavView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
		{
			if (args.IsSettingsInvoked)
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
			Type? page;
			if (navItemTag == "settings")
			{
				page = typeof(SettingsPage);
			}
			else
			{
				var item = _pages.FirstOrDefault(p =>
				{
					var (tag, _) = p;
					return tag.Equals(navItemTag);
				});
				page = item.Page;
			}

			// Get the page type before navigation so you can prevent duplicate
			// entries in the backstack.
			var preNavPageType = SettingsContentFrame.CurrentSourcePageType;

			// Only navigate if the selected page isn't currently loaded.
			if (!(page is null) && !Type.Equals(preNavPageType, page))
			{
				SettingsContentFrame.Navigate(page, null, transitionInfo);
			}
		}

		private void NavView_BackRequested(muxc.NavigationView sender, muxc.NavigationViewBackRequestedEventArgs args)
		{
			if (Frame.CanGoBack) Frame.GoBack();
		}

		public event PropertyChangedEventHandler? PropertyChanged;

		[NotifyPropertyChangedInvocator]
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class SettingsPageResources
	{
		private readonly TranslationManager _translation;

		public SettingsPageResources(TranslationManager manager)
		{
			_translation = manager;
		}

		public string Site => _translation.GetStringResource("siteManager");
		public string General => _translation.GetStringResource("general");
		public string Navigation => _translation.GetStringResource("navigation");
		public string Pages => _translation.GetStringResource("pages");
		public string Labels => _translation.GetStringResource("labels");
		public string Assets => _translation.GetStringResource("assets");
		public string Modules => _translation.GetStringResource("modules");
		public string Rendering => _translation.GetStringResource("rendering");
		public string Storage => _translation.GetStringResource("storage");
		public string Sync => _translation.GetStringResource("sync");
		public string System => _translation.GetStringResource("system");
		public string DeveloperTools => _translation.GetStringResource("developerTools");
		public string Personalization => _translation.GetStringResource("personalization");
	}
}