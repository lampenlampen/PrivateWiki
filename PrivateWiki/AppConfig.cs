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

		public async Task<StorageFolder> GetDataFolder()
		{
			var localFolder = ApplicationData.Current.LocalFolder;
			var dataFolder = await localFolder.CreateFolderAsync("data", CreationCollisionOption.OpenIfExists);

			return dataFolder;
		}
	}
}