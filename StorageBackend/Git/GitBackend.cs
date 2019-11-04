using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Contracts.Storage;
using LibGit2Sharp;
using Models.Pages;
using Models.Storage;

namespace StorageBackend.Git
{
	public class GitBackend : IStorageBackend, IMarkdownPageStorage, IDisposable
	{
		private GitStorage Storage { get; set; }
		
		private Repository Repo { get; set; }

		public GitBackend(GitStorage storage)
		{
			Storage = storage;
			
			// Create Repo
			var result = Create();
			result.Wait();
			if (result.Result)
			{
				throw new GitBackendException("Git Repo could not be created.");
			}
		}

		private async Task<bool> Create()
		{
			if (Repository.IsValid(Storage.Path)) return true;

			var localFolder = ApplicationData.Current.LocalFolder;
			var gitFolder = await localFolder.CreateFolderAsync(Storage.Path, CreationCollisionOption.OpenIfExists);


			Repo = new Repository(gitFolder.Path);

			return Repository.IsValid(Storage.Path);
		}

		public Task<bool> ExistsAsync()
		{
			return Task.Run(() => Repository.IsValid(Storage.Path));
		}

		public bool Delete()
		{
			throw new NotImplementedException();
		}

		public async Task<MarkdownPage> GetMarkdownPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<MarkdownPage> GetMarkdownPageAsync(string link)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<MarkdownPage>> GetAllMarkdownPagesAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdateMarkdownPage(MarkdownPage page, PageAction action)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DeleteMarkdownPageAsync(MarkdownPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DeleteMarkdownPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> InsertMarkdownPageAsync(MarkdownPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ContainsMarkdownPageAsync(MarkdownPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ContainsMarkdownPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ContainsMarkdownPageAsync(string link)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<PageHistory<MarkdownPage>>> GetMarkdownPageHistoryAsync(string pageLink)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			Repo?.Dispose();
		}

		~GitBackend() => Repo?.Dispose();
	}
}