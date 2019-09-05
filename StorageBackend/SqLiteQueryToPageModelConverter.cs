
/* Unmerged change from project 'DataAccessLibrary (netcoreapp3.0)'
Before:
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using NodaTime;
After:
using Microsoft.Data.Sqlite;
using NodaTime;
using System;
using Microsoft.Data.Generic;
using System.Text;
*/
using Microsoft.Data.Sqlite;
using NodaTime;
using System.Collections.Generic;

namespace DataAccessLibrary
{
	public static class SQLiteQueryToPageModelConverter
	{
		public static List<PageModel> ConvertSQLiteQueryToPageModels(SqliteDataReader query)
		{
			var pages = new List<PageModel>();

			while (query.Read())
			{
				var page = new PageModel(
					id: query.GetGuid(query.GetOrdinal(SQLiteHelper.PagesTable.col_id)),
					link: query.GetString(query.GetOrdinal(SQLiteHelper.PagesTable.col_link)),
					content: query.GetString(query.GetOrdinal(SQLiteHelper.PagesTable.col_content)),
					creationTime: Instant.FromUnixTimeMilliseconds(query.GetInt64(query.GetOrdinal(SQLiteHelper.PagesTable.col_creationDate))),
					changeTime: Instant.FromUnixTimeMilliseconds(query.GetInt64(query.GetOrdinal(SQLiteHelper.PagesTable.col_lastChangeDate))));

				page.IsFavorite = query.GetBoolean(query.GetOrdinal(SQLiteHelper.PagesTable.col_isFavorite));
				page.IsLocked = query.GetBoolean(query.GetOrdinal(SQLiteHelper.PagesTable.col_isLocked));

				var col_externalFileToken = query.GetOrdinal(SQLiteHelper.PagesTable.col_externalFileToken);
				if (query.IsDBNull(col_externalFileToken))
				{
					page.ExternalFileToken = null;
				}
				else
				{
					page.ExternalFileToken = query.GetString(col_externalFileToken);
				}

				var col_externalFileImportDate = query.GetOrdinal(SQLiteHelper.PagesTable.col_externalFileImportDate);
				if (query.IsDBNull(col_externalFileImportDate))
				{
					page.ExternalFileImportDate = null;
				}
				else
				{
					page.ExternalFileImportDate = Instant.FromUnixTimeMilliseconds(query.GetInt64(col_externalFileImportDate));
				}

				pages.Add(page);
			}

			return pages;
		}
	}
}
