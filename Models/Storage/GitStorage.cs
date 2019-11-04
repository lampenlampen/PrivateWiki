using System.IO;

namespace Models.Storage
{
	public class GitStorage : IStorage
	{
		public string RepoUrl { get; }
		
		public string Author_Name { get; }
		
		public string Author_Email { get; }

		public string Branch { get; } = "master";
		
		public string Path { get; }

		public GitStorage(string repoUrl, string repoPath, string authorName, string authorEmail)
		{
			RepoUrl = repoUrl;
			Path = repoPath;
			Author_Name = authorName;
			Author_Email = authorEmail;
		}
	}
}