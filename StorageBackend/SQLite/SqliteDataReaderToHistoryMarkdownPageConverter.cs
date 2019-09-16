using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;
using Models.Pages;
using NodaTime;

namespace StorageBackend.SQLite
{
	class SqliteDataReaderToHistoryMarkdownPageConverter : SqliteDataReaderToMarkdownPageConverter
	{
		public new static SqliteDataReaderToHistoryMarkdownPageConverter Instance => new SqliteDataReaderToHistoryMarkdownPageConverter();

		public bool ConvertToHistoryMarkdownPageModel(SqliteDataReader reader, HistoryMarkdownPage page)
		{
			while (reader.Read())
			{
				if (SqliteDataToMarkdownModel(reader, page))
				{
					page.ValidFrom = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_from")));
					page.ValidTo = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_to")));

					return true;
				}
			}

			return false;
		}

		public IList<HistoryMarkdownPage> ConvertToHistoryMarkdownPageModels(SqliteDataReader reader)
		{
			var pages = new List<HistoryMarkdownPage>();

			while (reader.Read())
			{
				var page = new HistoryMarkdownPage();
				if (SqliteDataToMarkdownModel(reader, page))
				{
					page.ValidFrom = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_from")));
					page.ValidTo = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_to")));

					pages.Add(page);
				}
			}

			return pages;
		}
	}
}
