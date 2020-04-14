using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;
using PrivateWiki.Settings;
using PrivateWiki.UI.Controls.Settings.Sync;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.UI.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class SyncSettingsPage : Page
	{
		private ObservableCollection<SyncModel> syncItems = new ObservableCollection<SyncModel>();

		public SyncSettingsPage()
		{
			this.InitializeComponent();
			LoadStorageModels();
		}

		private void SettingsHeader_OnApplyClick(object sender, RoutedEventArgs e)
		{
			SaveStorageModels();
		}

		private void SaveStorageModels()
		{
			var handler = new SyncModelHandler();
			handler.SaveSyncModels(syncItems);
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			LoadStorageModels();
		}

		private async void LoadStorageModels()
		{
			var handler = new SyncModelHandler();
			List<SyncModel> models;
			try
			{
				models = await handler.LoadSyncModels();
			}
			catch (Exception e)
			{
				models = new List<SyncModel>();
			}

			syncItems.Clear();
			foreach (var model in models) syncItems.Add(model);
		}

		private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var item = ((ListView) sender).SelectedItem;

			switch (item)
			{
				case LFSModel model:
					var control = new LFSControl();
					control.Init(model);
					SyncSettingsContent.Children.Add(control);
					break;
			}
		}

		private void AddLFSItem(object sender, RoutedEventArgs e)
		{
			syncItems.Add(new LFSModel());
		}
	}
}