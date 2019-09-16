using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Models.Storage;
using NodaTime;
using PrivateWiki.Data.DataAccess;
using PrivateWiki.Models;
using StorageBackend.SQLite;

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
	}
}