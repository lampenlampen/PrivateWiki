using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Controls.Settings.Storage;
using PrivateWiki.Models;
using PrivateWiki.Settings;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PrivateWiki.Pages.SettingsPages
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class StorageSettingsPage : Page
	{
		private ObservableCollection<StorageModel> storageItems = new ObservableCollection<StorageModel>();

		public StorageSettingsPage()
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
			var handler = new StorageModelHandler();
			handler.SaveStorageModels(storageItems);
		}

		private void SettingsHeader_OnResetClick(object sender, RoutedEventArgs e)
		{
			LoadStorageModels();
		}

		private async void LoadStorageModels()
		{
			var handler = new StorageModelHandler();
			List<StorageModel> models;
			try
			{
				models = await handler.LoadStorageModels();
			}
			catch (Exception e)
			{
				models = new List<StorageModel>();
			}

			storageItems.Clear();
			foreach (var model in models) storageItems.Add(model);
		}

		private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var item = ((ListView) sender).SelectedItem;

			switch (item)
			{
				case LFSModel model:
					var control = new LFSControl();
					control.Init(model);
					StorageSettingsContent.Children.Add(control);
					break;
			}
		}

		private void AddLFSItem(object sender, RoutedEventArgs e)
		{
			storageItems.Add(new LFSModel());
		}
	}
}
