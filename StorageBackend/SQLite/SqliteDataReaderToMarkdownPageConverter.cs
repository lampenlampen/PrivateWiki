using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Models.Pages;
using NodaTime;

namespace StorageBackend.SQLite
{
	public class SqliteDataReaderToMarkdownPageConverter
	{
		public static SqliteDataReaderToMarkdownPageConverter Instance => new SqliteDataReaderToMarkdownPageConverter();

		public MarkdownPage? ConvertToPageModel(SqliteDataReader reader)
		{
			while (reader.Read())
			{
				var id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
				var link = reader.GetString(reader.GetOrdinal("link"));
				var content = reader.GetString(reader.GetOrdinal("content"));
				var created = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("created")));
				var changed = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("changed")));
				var locked = reader.GetBoolean(reader.GetOrdinal("locked"));
				var page = new MarkdownPage(link, id, content, created, changed, locked);

				return page;

			}

			return null;
		}

		public IList<MarkdownPage> ConvertToPageModels(SqliteDataReader reader)
		{
			var pages = new List<MarkdownPage>();
			
			while (reader.Read())
			{
				var id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
				var link = reader.GetString(reader.GetOrdinal("link"));
				var content = reader.GetString(reader.GetOrdinal("content"));
				var created = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("created")));
				var changed = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("changed")));
				var locked = reader.GetBoolean(reader.GetOrdinal("locked"));
				var page = new MarkdownPage(link, id, content, created, changed, locked);
				
				pages.Add(page);
			}

			return pages;
		}
	}
}