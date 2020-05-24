namespace PrivateWiki.StorageBackend
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