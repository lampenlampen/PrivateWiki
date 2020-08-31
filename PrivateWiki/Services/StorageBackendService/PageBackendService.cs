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

		public Task<GenericPage> GetPageAsync(Guid id)
		{
			return _storage.GetPageAsync(id);
		}

		public Task<GenericPage> GetPageAsync(string link)
		{
			return _storage.GetPageAsync(link);
		}

		public Task<IEnumerable<GenericPage>> GetAllPagesAsync()
		{
			return _storage.GetAllPagesAsync();
		}

		public Task<bool> UpdatePage(GenericPage page, PageAction action)
		{
			return _storage.UpdatePage(page, action);
		}

		public Task<bool> DeletePageAsync(GenericPage page)
		{
			return _storage.DeletePageAsync(page);
		}

		public Task<bool> DeletePageAsync(Guid id)
		{
			return _storage.DeletePageAsync(id);
		}

		public Task<bool> InsertPageAsync(GenericPage page)
		{
			return _storage.InsertPageAsync(page);
		}

		public Task<bool> ContainsPageAsync(GenericPage page)
		{
			return _storage.ContainsPageAsync(page);
		}

		public Task<bool> ContainsPageAsync(Guid id)
		{
			return _storage.ContainsPageAsync(id);
		}

		public Task<bool> ContainsPageAsync(string link)
		{
			return _storage.ContainsPageAsync(link);
		}

		public Task<IEnumerable<GenericPageHistory>> GetPageHistoryAsync(string pageLink)
		{
			return _storage.GetPageHistoryAsync(pageLink);
		}

		public Task<bool> InsertLabelAsync(Label label)
		{
			return _storage.InsertLabelAsync(label);
		}

		public Task<Label> GetLabelAsync(Guid id)
		{
			return _storage.GetLabelAsync(id);
		}

		public Task<IEnumerable<Label>> GetAllLabelsAsync()
		{
			return _storage.GetAllLabelsAsync();
		}

		public Task<bool> DeleteLabelAsync(Guid id)
		{
			return _storage.DeleteLabelAsync(id);
		}

		public Task<bool> DeleteLabelAsync(Label label)
		{
			return _storage.DeleteLabelAsync(label);
		}
	}
}