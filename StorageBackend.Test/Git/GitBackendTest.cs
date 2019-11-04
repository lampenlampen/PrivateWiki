using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StorageBackend.Test.SqLite;

namespace StorageBackend.Test.Git
{
	[TestClass]
	public class GitBackendTest
	{
		[TestMethod]
		public void LibGit2SharpTest()
		{
			var path = Directory.GetCurrentDirectory();
			var dir = Directory.CreateDirectory(Path.Combine(path, "gitrepotest"));

			if (!Repository.IsValid(dir.FullName))
			{
				Repository.Clone("https://gitlab.com/lampenlampen/gitbackendtest.git", dir.FullName);
			}

			var repo = new Repository(dir.FullName);

			var filePath = Path.Combine(dir.FullName, "test.txt");

			var writer = File.CreateText(filePath);
			writer.Write("Test File test.txtasdasdasd123");
			writer.Flush();

			Commands.Stage(repo, "*");


			// Create the committer's signature and commit
			Signature author = new Signature("Felix Zacher", "felix.zacher@outlook.de", DateTime.Now);
			Signature committer = author;

			// Commit to the repository
			var repoStatus = repo.RetrieveStatus();
			if (repoStatus.IsDirty)
			{
				var commit = repo.Commit("Here's a commit i made3!", author, committer);
			}
			
			var options = new PushOptions()
			{
				CredentialsProvider = (_url, _user, _cred) => new UsernamePasswordCredentials
				{
					Username = GitAccessTokenForTesting.Gitlab_Username,
					Password = GitAccessTokenForTesting.Gitlab_Access_Token
				},
			};

			repo.Network.Push(repo.Branches["master"], options);
		}
	}
}