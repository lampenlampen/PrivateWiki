using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using NodaTime;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.UWP.StorageBackend.SQLite
{
	class SqliteDataReaderToHistoryMarkdownPageConverter : SqliteDataReaderToMarkdownPageConverter
	{
		public new static SqliteDataReaderToHistoryMarkdownPageConverter Instance =>
			new SqliteDataReaderToHistoryMarkdownPageConverter();

		public IEnumerable<MarkdownPageHistory> ConvertToHistoryMarkdownPageModels(SqliteDataReader reader)
		{
			var pages = new List<MarkdownPageHistory>();

			while (reader.Read())
			{
				var page = new MarkdownPage();
				if (SqliteDataToMarkdownModel(reader, page))
				{
					var action = (PageAction) reader.GetInt32(reader.GetOrdinal("action"));

					var mdPageHistory = new MarkdownPageHistory(page)
					{
						ValidFrom =
							Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_from"))),
						ValidTo = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_to"))),
						Action = (PageAction) reader.GetInt16(reader.GetOrdinal("action"))
					};

					pages.Add(mdPageHistory);
				}
			}

			return pages;
		}
	}
}