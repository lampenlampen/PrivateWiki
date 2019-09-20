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

		public bool ConvertToMarkdownPageModel(SqliteDataReader reader, MarkdownPage page)
		{
			while (reader.Read())
			{
				return SqliteDataToMarkdownModel(reader, page);
			}

			return false;
		}

		protected bool SqliteDataToMarkdownModel(SqliteDataReader reader, MarkdownPage page)
		{
			page.Id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
			page.Link = reader.GetString(reader.GetOrdinal("link"));
			page.Content = reader.GetString(reader.GetOrdinal("content"));
			page.Created = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("created")));
			page.LastChanged = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("changed")));
			page.IsLocked = reader.GetBoolean(reader.GetOrdinal("locked"));

			return true;
		}

		public IList<MarkdownPage> ConvertToPageModels(SqliteDataReader reader)
		{
			var pages = new List<MarkdownPage>();
			
			while (reader.Read())
			{
				var page = new MarkdownPage();
				if (SqliteDataToMarkdownModel(reader, page)) pages.Add(page);
			}

			return pages;
		}
	}
}