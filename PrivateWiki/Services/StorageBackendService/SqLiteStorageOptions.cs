namespace PrivateWiki.Services.StorageBackendService
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