namespace PrivateWiki.DataModels
{
	public class Folder
	{
		public string Name { get; }
		
		public string Path { get; }

		public Folder(string path, string name)
		{
			Path = path;
			Name = name;
		}
	}
}