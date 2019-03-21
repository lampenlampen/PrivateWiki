using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using NodaTime;

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
					id: query.GetGuid(query.GetOrdinal(SQLiteHelper.col_id)),
					link: query.GetString(query.GetOrdinal(SQLiteHelper.col_link)),
					content: query.GetString(query.GetOrdinal(SQLiteHelper.col_content)),
					creationTime: Instant.FromUnixTimeMilliseconds(query.GetInt64(query.GetOrdinal(SQLiteHelper.col_creationDate))),
					changeTime: Instant.FromUnixTimeMilliseconds(query.GetInt64(query.GetOrdinal(SQLiteHelper.col_lastChangeDate))));

				page.IsFavorite = query.GetBoolean(query.GetOrdinal(SQLiteHelper.col_isFavorite));
				page.IsLocked = query.GetBoolean(query.GetOrdinal(SQLiteHelper.col_isLocked));

				var col_externalFileToken = query.GetOrdinal(SQLiteHelper.col_externalFileToken);
				if (query.IsDBNull(col_externalFileToken))
				{
					page.ExternalFileToken = null;
				}
				else
				{
					page.ExternalFileToken = query.GetString(col_externalFileToken);
				}

				var col_externalFileImportDate = query.GetOrdinal(SQLiteHelper.col_externalFileImportDate);
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
