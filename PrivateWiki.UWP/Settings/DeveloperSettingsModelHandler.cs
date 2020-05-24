using System;
using Windows.Storage;
using PrivateWiki.Models;

#nullable enable

namespace PrivateWiki.Settings
{
	class DeveloperSettingsModelHandler
	{
		public bool SaveDeveloperSettingsModel(DeveloperSettingsModel model)
		{
			var container = GetRenderingSettingsContainer();

			container.Values["experimental_is_acrylicbackground_enabled"] = model.IsAcrylicBackgroundEnabled;

			return true;
		}

		public DeveloperSettingsModel LoadDeveloperSettingsModel()
		{
			var container = GetRenderingSettingsContainer();
			DeveloperSettingsModel model;

			try
			{
				model = new DeveloperSettingsModel
				{
					IsAcrylicBackgroundEnabled = (bool)container.Values["experimental_is_acrylicbackground_enabled"]
				};
			}
			catch (NullReferenceException e)
			{
				model = new DeveloperSettingsModel();
			}

			return model;
		}

		private ApplicationDataContainer GetRenderingSettingsContainer()
		{
			var roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
			var settingsContainer =
				roamingSettings.CreateContainer("settings", ApplicationDataCreateDisposition.Always);

			var developerSettingsContainer =
				settingsContainer.CreateContainer("dev_settings", ApplicationDataCreateDisposition.Always);

			return developerSettingsContainer;
		}
	}
}
