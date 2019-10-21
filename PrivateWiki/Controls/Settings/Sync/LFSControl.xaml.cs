using System;
using System.IO;
using System.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Markdig.Extensions.Yaml;
using Markdig.Syntax;
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

		private async void DoLightSync(object sender, RoutedEventArgs e)
		{
			// TODO Light Sync
			await new LFSSyncActions().ExportTask2(Model);

			App.Current.manager.ShowOperationFinishedNotification();
		}

		private async void ImportData(object sender, RoutedEventArgs e)
		{
			var deserializer = new MarkdownDocToMarkdownPageDeserializer();

			var folder = StorageApplicationPermissions.FutureAccessList.GetFolderAsync(Model.TargetToken);

			var files = (await folder).GetFilesAsync(CommonFileQuery.DefaultQuery);

			foreach (var file in await files)
			{
				var content = await FileIO.ReadTextAsync(file);

				var markdig = new Markdig.Markdig();
				var doc = markdig.Parse(content);
				var yamlBlock = doc.Descendants<YamlFrontMatterBlock>().FirstOrDefault();

				if (yamlBlock != null)
				{
					var yaml = content.Substring(yamlBlock.Span.Start, yamlBlock.Span.Length);
					var a = deserializer.Deserialize(new StringReader(yaml));
				}
				
				
			}


			
		}
	}
}