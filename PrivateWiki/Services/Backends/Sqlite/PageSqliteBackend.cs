using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.StorageServices.Sql.Sqlite;

namespace PrivateWiki.Services.Backends.Sqlite
{
	public class PageSqliteBackend : IPageBackend
	{
		private ISqliteStorage _sqliteStorage;

		public PageSqliteBackend(ISqliteStorage sqliteStorage)
		{
			_sqliteStorage = sqliteStorage;
		}

		public async Task InsertLabelAsync(Label label)
		{
			throw new NotImplementedException();
		}

		public async Task<Label> GetLabelAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Label>> GetAllLabelsAsync()
		{
			throw new NotImplementedException();
		}

		public async Task DeleteLabelAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task UpdateLabelAsync(Label label)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Label>> GetLabelsForPage(Guid pageId)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<Label>> GetLabelsForPage(GenericPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<GenericPage> GetPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<GenericPage> GetPageAsync(string link)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<GenericPage>> GetAllPagesAsync()
		{
			throw new NotImplementedException();
		}

		public async Task<bool> UpdatePage(GenericPage page, PageAction action)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DeletePageAsync(GenericPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> DeletePageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> InsertPageAsync(GenericPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ContainsPageAsync(GenericPage page)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ContainsPageAsync(Guid id)
		{
			throw new NotImplementedException();
		}

		public async Task<bool> ContainsPageAsync(string link)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<GenericPageHistory>> GetPageHistoryAsync(string pageLink)
		{
			throw new NotImplementedException();
		}
	}
}