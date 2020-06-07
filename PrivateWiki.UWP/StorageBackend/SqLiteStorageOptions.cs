namespace PrivateWiki.UWP.StorageBackend
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