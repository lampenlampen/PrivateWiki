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
		public new static SqliteDataReaderToHistoryMarkdownPageConverter Instance =>
			new SqliteDataReaderToHistoryMarkdownPageConverter();

		public List<MarkdownPageHistory> ConvertToHistoryMarkdownPageModels(SqliteDataReader reader)
		{
			var pages = new List<MarkdownPageHistory>();

			while (reader.Read())
			{
				var page = new MarkdownPage();
				if (SqliteDataToMarkdownModel(reader, page))
				{
					var mdPageHistory = new MarkdownPageHistory(page)
					{
						ValidFrom =
							Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_from"))),
						ValidTo = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_to"))),
						Action = (PageAction) Enum.Parse(typeof(PageAction),
							reader.GetString(reader.GetOrdinal("action")))
					};


					pages.Add(mdPageHistory);
				}
			}

			return pages;
		}
	}
}