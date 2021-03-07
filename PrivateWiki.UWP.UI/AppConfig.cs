using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace PrivateWiki.UWP.UI
{
	public class AppConfig
	{
		public static readonly string DataFolderName = "data";

		private readonly Lazy<Task<StorageFolder>> _dataFolder = new Lazy<Task<StorageFolder>>(() =>
		{
			var localFolder = ApplicationData.Current.LocalFolder;
			var dataFolder = localFolder.CreateFolderAsync(DataFolderName, CreationCollisionOption.OpenIfExists);

			return dataFolder.AsTask();
		});

		public Task<StorageFolder> GetDataFolderAsync()
		{
			return _dataFolder.Value;
		}
	}
}