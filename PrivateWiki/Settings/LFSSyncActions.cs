using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Models.Storage;
using NodaTime;
using PrivateWiki.Models;
using PrivateWiki.Storage;
using StorageBackend.SQLite;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace PrivateWiki.Settings
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
					.WithTypeConverter(new InstantYamlConverter())
					.Build();

				var a = serializer.Serialize(page);
				await FileIO.WriteTextAsync(await file, a);
			}
		}
	}

	class InstantYamlConverter : IYamlTypeConverter
	{
		public bool Accepts(Type type)
		{
			if (typeof(Instant) == type)
			{
				return true;
			}

			return false;
		}

		public object ReadYaml(IParser parser, Type type)
		{
			throw new NotImplementedException();
		}

		public void WriteYaml(IEmitter emitter, object value, Type type)
		{
			if (type == typeof(Instant))
			{
				var instant = (Instant) value;

				emitter.Emit(new Scalar(instant.ToUnixTimeMilliseconds().ToString()));
			}
		}
	}
}