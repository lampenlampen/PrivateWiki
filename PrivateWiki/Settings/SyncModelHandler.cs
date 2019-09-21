using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PrivateWiki.Models;

namespace PrivateWiki.Settings
{
	class SyncModelHandler
	{
		public async void SaveSyncModels(IEnumerable<SyncModel> models)
		{
			var storageSettingsFile = await GetStorageFile();
			using var stream = await storageSettingsFile.OpenStreamForWriteAsync();

			var writer = new JsonTextWriter(new StreamWriter(stream));

			writer.Formatting = Formatting.Indented;
			writer.WriteStartObject();
			writer.WritePropertyName("version");
			writer.WriteValue("0.1");
			writer.WritePropertyName("storage_items");
			writer.WriteStartArray();

			var storageItemSerializer = new SyncModelJsonSerializer(writer);

			foreach (var item in models)
			{
				storageItemSerializer.WriteJson(item);
			}

			writer.WriteEndArray();
			writer.WriteEndObject();

			writer.Flush();
		}

		public async Task<List<SyncModel>> LoadSyncModels()
		{
			var models = new List<SyncModel>();

			var localFolder = ApplicationData.Current.LocalFolder;
			var settingsFolder = await localFolder.CreateFolderAsync("settings", CreationCollisionOption.OpenIfExists);
			var file = await settingsFolder.GetFileAsync("storage.json");

			var json = await FileIO.ReadTextAsync(file);

			JObject rss = JObject.Parse(json);

			var version = (string) rss["version"];

			JArray items = (JArray) rss["storage_items"];

			foreach (var item in items)
			{
				var type = (string) item["type"];

				switch (type)
				{
					case "local_file_system":
						var model = new LFSModel
						{
							Title = (string) item["title"],
							Subtitle = (string) item["subtitle"],
							TargetToken = (string) item["target_token"],
							TargetPath = (string) item["target_path"],
							SyncFrequency = Enum.Parse<SyncFrequency>((string) item["sync_frequency"])
						};
						models.Add(model);
						break;
				}
			}

			return models;
		}

		private async Task<IStorageFile> GetStorageFile()
		{
			var localFolder = ApplicationData.Current.LocalFolder;
			var settingsFolder = await localFolder.CreateFolderAsync("settings", CreationCollisionOption.OpenIfExists);
			return await settingsFolder.CreateFileAsync("storage.json", CreationCollisionOption.ReplaceExisting);

		}
	}

	class SyncModelJsonSerializer
	{
		private JsonWriter Writer { get; set; }

		public SyncModelJsonSerializer(JsonWriter writer)
		{
			Writer = writer;
		}

		public JsonWriter WriteJson(SyncModel model)
		{
			switch (model)
			{
				case LFSModel lfsModel:
					LFSModelWriteJson(Writer, lfsModel);
				break;
			}

			return Writer;
		}

		private void LFSModelWriteJson(JsonWriter writer, LFSModel model)
		{
			writer.WriteStartObject();
			writer.WritePropertyName("type");
			writer.WriteValue("local_file_system");
			writer.WritePropertyName("title");
			writer.WriteValue(model.Title);
			writer.WritePropertyName("subtitle");
			writer.WriteValue(model.Subtitle);
			writer.WritePropertyName("target_token");
			writer.WriteValue(model.TargetToken);
			writer.WritePropertyName("target_path");
			writer.WriteValue(model.TargetPath);
			writer.WritePropertyName("sync_frequency");
			writer.WriteValue(model.SyncFrequency.ToString());
		}
	}
}
