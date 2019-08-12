using DataAccessLibrary;
using NodaTime;
using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;

namespace PrivateWiki.Data
{
	public class MarkdownImport
	{
		public async Task<PageModel> ImportMarkdownFileAsync(StorageFile file)
		{
			var token = StorageApplicationPermissions.FutureAccessList.Add(file);

			var content = await ReadMarkdownFileAsync(file);

			return new PageModel(Guid.NewGuid(), file.DisplayName, content, SystemClock.Instance, token, SystemClock.Instance.GetCurrentInstant());
		}

		private async Task<string> ReadMarkdownFileAsync(StorageFile file)
		{
			return await FileIO.ReadTextAsync(file);
		}
	}
}