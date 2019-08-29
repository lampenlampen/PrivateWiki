using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;

#nullable enable

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls.Settings.Storage
{
	public sealed partial class LFSControl : UserControl
	{
		private LFSModel Model { get; set; }

		public LFSControl()
		{
			this.InitializeComponent();
		}

		public void Init(LFSModel model)
		{
			Model = model;
			Bindings.Update();
		}

		private async void SelectSyncTarget(object sender, RoutedEventArgs e)
		{
			var picker = new FolderPicker();
			picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
			picker.FileTypeFilter.Add("*");

			
			StorageFolder? folder = await picker.PickSingleFolderAsync();

			if (folder != null)
			{
				var token = Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(folder);
				Model.TargetToken = token;
				Model.TargetPath = folder.Path;
				//TargetPath.Text = Model.TargetPath;
			}


		}

		private void SyncFrequencyChecked(object sender, RoutedEventArgs e)
		{
			var radiobutton = (RadioButton) sender;

			switch (radiobutton.Content)
			{
				case "Hourly":
					Model.SyncFrequency = SyncFrequency.Hourly;
					break;
				case "Daily": Model.SyncFrequency = SyncFrequency.Daily;
					break;
				case "Weekly": Model.SyncFrequency = SyncFrequency.Weekly;
					break;
			}
		}

		private void DoForceSync(object sender, RoutedEventArgs e)
		{
			// TODO Force Sync
		}

		private void DoLightSync(object sender, RoutedEventArgs e)
		{
			// TODO Light Sync
		}
	}
}
