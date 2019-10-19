using System;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PrivateWiki.Models;
using PrivateWiki.Settings;
using PrivateWiki.Utilities.ExtensionFunctions;

#nullable enable

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace PrivateWiki.Controls.Settings.Sync
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

			Model.SyncFrequency = radiobutton.Content switch
			{
				"Never" => SyncFrequency.Never,
				"Hourly" => SyncFrequency.Hourly,
				"Daily" => SyncFrequency.Daily,
				"Weekly" => SyncFrequency.Weekly,
				_ => Model.SyncFrequency
			};
		}

		private void DoForceSync(object sender, RoutedEventArgs e)
		{
			// TODO Force Sync

			var task = new LFSSyncActions().ForceSyncTask(Model);
		}

		private async void DoLightSync(object sender, RoutedEventArgs e)
		{
			// TODO Light Sync
			await new LFSSyncActions().ExportTask(Model);
			
			App.Current.manager.ShowOperationFinishedNotification();
		}
	}
}