namespace PrivateWiki.DataModels
{
	public class File
	{
		public string Path { get; }

		public File(string path)
		{
			Path = path;
		}
	}
}