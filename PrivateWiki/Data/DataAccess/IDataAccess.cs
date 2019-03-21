using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLibrary;

namespace PrivateWiki.Data.DataAccess
{
	interface IDataAccess
	{
		void InitializeDatabase();
		void InsertPage(PageModel page);
		void InsertPages(IEnumerable<PageModel> pages);
		List<PageModel> GetPages();
		PageModel GetPageOrNull(Guid id);
		void UpdatePage(PageModel page);
		void UpdateContent(PageModel page);
		bool ContainsPage(PageModel page);
		bool ContainsPage(Guid id);
		bool ContainsPage(string link);
		bool DeletePage(PageModel page);
	}
}
