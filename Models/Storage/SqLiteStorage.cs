namespace Models.Storage
{
	public class SqLiteStorage : Storage
	{
		public SqLiteStorage(string filename)
		{
			Filename = filename;
		}

		public string Filename { get; }
		
	}
}