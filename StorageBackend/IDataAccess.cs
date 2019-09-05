using System;
using System.Collections.Generic;

namespace StorageBackend
{
	public interface IDataAccess
	{
		void InitializeDatabase();
		void InsertPage(PageModel page);
		void InsertPages(IEnumerable<PageModel> pages);
		List<PageModel> GetPages();
		PageModel GetPageOrNull(Guid id);
		PageModel GetPageOrNull(string link);
		void UpdatePage(PageModel page);
		void UpdateContent(PageModel page);
		bool ContainsPage(PageModel page);
		bool ContainsPage(Guid id);
		bool ContainsPage(string link);
		bool DeletePage(PageModel page);
	}
}