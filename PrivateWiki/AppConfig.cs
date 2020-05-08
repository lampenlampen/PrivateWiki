using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using PrivateWiki.Data;

namespace PrivateWiki
{
	public class AppConfig
	{
		public static readonly IList<ContentType> SupportedContentTypes2 = new List<ContentType> {ContentType.Html, ContentType.Markdown, ContentType.Text};

		private readonly Lazy<Task<StorageFolder>> _dataFolder = new Lazy<Task<StorageFolder>>(() =>
		{
			var localFolder = ApplicationData.Current.LocalFolder;
			var dataFolder = localFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);

			return dataFolder.AsTask();
		});

		public async Task<StorageFolder> GetDataFolderAsync()
		{
			return await _dataFolder.Value;
		}
	}
}