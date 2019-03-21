using System;
using System.Collections;
using System.Collections.Generic;
using Windows.Media.Protection.PlayReady;
using DataAccessLibrary;

namespace PrivateWiki.Data
{
	internal interface IPageAccess : IDataAccess
	{
		void InitializeDatabase();

		List<PageModel> GetAllPages();

		PageModel GetPageOrNull(Guid id);

		bool ContainsPage(Guid id);

		bool ContainsPage(PageModel page);

		void InsertPage(PageModel page);

		void UpdatePage(PageModel page);

		void DeletePage(PageModel page);

		void DeletePage(Guid id);
	}
}