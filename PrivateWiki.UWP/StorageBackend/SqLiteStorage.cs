namespace PrivateWiki.UWP.StorageBackend
{
	public class SqLiteStorage : IStorage
	{
		public SqLiteStorage(string filename)
		{
			Filename = filename;
		}

		public string Filename { get; }
	}
}