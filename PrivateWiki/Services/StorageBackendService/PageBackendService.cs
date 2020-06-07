using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.StorageBackendService
{
	public class PageBackendService : IPageBackendService
	{
		private readonly IPageStorageBackendServiceImpl _storage;

		public PageBackendService(IPageStorageBackendServiceImpl storage)
		{
			_storage = storage;
		}

		public async Task<GenericPage> GetPageAsync(Guid id)
		{
			return await _storage.GetPageAsync(id);
		}

		public async Task<GenericPage> GetPageAsync(string link)
		{
			return await _storage.GetPageAsync(link);
		}

		public async Task<IEnumerable<GenericPage>> GetAllPagesAsync()
		{
			return await _storage.GetAllPagesAsync();
		}

		public async Task<bool> UpdatePage(GenericPage page, PageAction action)
		{
			return await _storage.UpdatePage(page, action);
		}

		public async Task<bool> DeletePageAsync(GenericPage page)
		{
			return await _storage.DeletePageAsync(page);
		}

		public async Task<bool> DeletePageAsync(Guid id)
		{
			return await _storage.DeletePageAsync(id);
		}

		public async Task<bool> InsertPageAsync(GenericPage page)
		{
			return await _storage.InsertPageAsync(page);
		}

		public async Task<bool> ContainsPageAsync(GenericPage page)
		{
			return await _storage.ContainsPageAsync(page);
		}

		public async Task<bool> ContainsPageAsync(Guid id)
		{
			return await _storage.ContainsPageAsync(id);
		}

		public async Task<bool> ContainsPageAsync(string link)
		{
			return await _storage.ContainsPageAsync(link);
		}

		public async Task<IEnumerable<GenericPageHistory>> GetPageHistoryAsync(string pageLink)
		{
			return await _storage.GetPageHistoryAsync(pageLink);
		}
	}
}