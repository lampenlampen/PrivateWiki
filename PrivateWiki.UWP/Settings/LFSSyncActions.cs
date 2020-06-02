using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using NodaTime;
using PrivateWiki.Models.Pages;
using PrivateWiki.UWP.Models;
using PrivateWiki.UWP.StorageBackend;
using PrivateWiki.UWP.StorageBackend.SQLite;
using YamlDotNet.RepresentationModel;
using YamlDotNet.Serialization;
using UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding;

namespace PrivateWiki.UWP.Settings
{
	class LFSSyncActions
	{
		public async void ForceSyncAsync(LFSModel model)
		{
			var storage = new SqLiteBackend(new SqLiteStorage("test"), SystemClock.Instance);
			var pages = await storage.GetAllMarkdownPagesAsync();

			var folder = await Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.GetFolderAsync(model.TargetToken);

			Parallel.ForEach(pages, async page =>
			{
				var file = await folder.CreateFileAsync($"{page.Link.Replace(':', '_')}.md", CreationCollisionOption.ReplaceExisting);
				FileIO.WriteTextAsync(file, page.Content, UnicodeEncoding.Utf8);
			});
		}

		public Task ForceSyncTask(LFSModel model)
		{
			void Action(object o) => ForceSyncAsync((LFSModel) o);

			var task = Task.Factory.StartNew(Action, model);
			return task;
		}

		public async Task ExportTask(LFSModel model)
		{
			var storage = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			var pages = storage.GetAllMarkdownPagesAsync();

			var folder = StorageApplicationPermissions.FutureAccessList.GetFolderAsync(model.TargetToken);

			foreach (var page in await pages)
			{
				var file = (await folder).CreateFileAsync($"{page.Link.Replace(':', '_')}.yml", CreationCollisionOption.ReplaceExisting);

				var serializer = new SerializerBuilder()
					.Build();

				var a = serializer.Serialize(page);
				await FileIO.WriteTextAsync(await file, a);
			}
		}

		public async Task ExportTask2(LFSModel model)
		{
			var storage = new SqLiteBackend(DefaultStorageBackends.GetSqliteStorage(), SystemClock.Instance);
			var pages = storage.GetAllMarkdownPagesAsync();

			var folder = StorageApplicationPermissions.FutureAccessList.GetFolderAsync(model.TargetToken);

			foreach (var page in await pages)
			{
				var file = (await folder).CreateFileAsync($"{page.Link.Replace(':', '_')}.md", CreationCollisionOption.ReplaceExisting);

				var writer = new StringWriter();
				writer.WriteLine("---");

				new MarkdownPageToMarkdownDocSerializer().Serialize(writer, page);

				// TODO Refactor see https://github.com/aaubry/YamlDotNet/issues/436#issuecomment-544276681
				var yaml = writer.ToString();
				var yaml2 = $"{yaml.Substring(0, yaml.Length - 6)}\n---";

				var stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(yaml2);
				stringBuilder.Append(page.Content);

				FileIO.WriteTextAsync(await file, stringBuilder.ToString());
			}
		}
	}

	class MarkdownPageToMarkdownDocSerializer
	{
		public void Serialize(TextWriter writer, MarkdownPage page)
		{
			var yamlStream = new YamlStream(new YamlDocument(new YamlMappingNode(
				new YamlScalarNode("id"), new YamlScalarNode(page.Id.ToString()),
				new YamlScalarNode("created"), new YamlScalarNode(page.Created.ToUnixTimeMilliseconds().ToString()),
				new YamlScalarNode("lastChanged"), new YamlScalarNode(page.LastChanged.ToUnixTimeMilliseconds().ToString()),
				new YamlScalarNode("locked"), new YamlScalarNode(page.IsLocked.ToString()),
				new YamlScalarNode("path"), new YamlScalarNode(page.Path.FullPath))));

			yamlStream.Save(writer, assignAnchors: true);
		}
	}

	class MarkdownDocToMarkdownPageDeserializer
	{
		public MarkdownPage Deserialize(TextReader reader)
		{
			var yamlStream = new YamlStream();
			yamlStream.Load(reader);

			var mapping = (YamlMappingNode) yamlStream.Documents[0].RootNode;

			var stringBuilder = new StringBuilder();

			foreach (var entry in mapping.Children)
			{
				stringBuilder.AppendLine(entry.ToString());
			}

			return new MarkdownPage();
		}
	}
}