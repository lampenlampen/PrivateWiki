using System;
using System.Collections.Generic;
using Windows.Foundation.Metadata;
using StorageBackend;
using SystemClock = NodaTime.SystemClock;

namespace PrivateWiki.Data.DataAccess
{
	[Obsolete("Use StorageBackends instead", true)]
	class DataAccessImpl : IDataAccess
	{
		private readonly StorageBackend.IDataAccess _dataAccess;

		public DataAccessImpl()
		{
			_dataAccess = new StorageBackend.DataAccess(SQLiteHelper.GetDBConnection(), SystemClock.Instance);
		}

		public void InitializeDatabase()
		{
			_dataAccess.InitializeDatabase();
		}

		public void InsertPage(PageModel page)
		{
			_dataAccess.InsertPage(page);
		}

		public void InsertPages(IEnumerable<PageModel> pages)
		{
			_dataAccess.InsertPages(pages);
		}

		public List<PageModel> GetPages()
		{
			return _dataAccess.GetPages();
		}

		public PageModel GetPageOrNull(Guid id)
		{
			return _dataAccess.GetPageOrNull(id);
		}

		public PageModel GetPageOrNull(string link)
		{
			return _dataAccess.GetPageOrNull(link);
		}

		public void UpdatePage(PageModel page)
		{
			_dataAccess.UpdatePage(page);
		}

		public void UpdateContent(PageModel page)
		{
			_dataAccess.UpdateContent(page);
		}

		public bool ContainsPage(PageModel page)
		{
			return _dataAccess.ContainsPage(page);
		}

		public bool ContainsPage(Guid id)
		{
			return _dataAccess.ContainsPage(id);
		}

		public bool ContainsPage(string link)
		{
			return _dataAccess.ContainsPage(link);
		}

		public bool DeletePage(PageModel page)
		{
			return _dataAccess.DeletePage(page);
		}
	}
}
