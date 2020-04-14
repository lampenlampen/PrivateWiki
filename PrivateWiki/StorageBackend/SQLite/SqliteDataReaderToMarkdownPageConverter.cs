using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using NodaTime;
using PrivateWiki.Models.Pages;

namespace PrivateWiki.StorageBackend.SQLite
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
			page.Content = reader.GetString(reader.GetOrdinal("content"));
			page.Created = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("created")));
			page.LastChanged = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("changed")));
			page.IsLocked = reader.GetBoolean(reader.GetOrdinal("locked"));

			var link = reader.GetString(reader.GetOrdinal("link"));
			var path = link.Split(new[] {':'}, StringSplitOptions.RemoveEmptyEntries);

			if (path.Length > 1)
			{
				string[] path2 = new string[path.Length - 1];
				Array.Copy(path, path2, path.Length - 1);

				page.Path = Path.of(path2, path[path.Length - 1]);
			}
			else
			{
				page.Path = Path.ofLink(link);
			}

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