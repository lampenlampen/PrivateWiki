﻿using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.UWP.Models;
using PrivateWiki.UWP.Settings;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UWP.UI.Pages.SettingsPagesOld
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class DeveloperSettingsPage : Page
	{
		private DeveloperSettingsModel Model { get; set; }

		public DeveloperSettingsPage()
		{
			this.InitializeComponent();
			ApplicationData.Current.DataChanged += RoamingDataChanged;

			if (Model == null)
			{
				LoadModel();
			}
		}

		private void RoamingDataChanged(ApplicationData sender, object args)
		{
			LoadModel();
		}


		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			var handler = new DeveloperSettingsModelHandler();
			handler.SaveDeveloperSettingsModel(Model);
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			LoadModel();
		}

		private void LoadModel()
		{
			var handler = new DeveloperSettingsModelHandler();
			Model = handler.LoadDeveloperSettingsModel();
			Bindings.Update();
		}
	}
}