namespace PrivateWiki.Services.StorageBackendService.SQLite
{
	public class SqLiteStorageOptions
	{
		public SqLiteStorageOptions(string filename)
		{
			Filename = filename;
		}

		public string Filename { get; }
	}
}