using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using Models.Pages;
using NodaTime;

namespace PrivateWiki.StorageBackend.SQLite
{
	class SqliteDataReaderToHtmlPageConverter
	{
		public static SqliteDataReaderToHtmlPageConverter Instance => new SqliteDataReaderToHtmlPageConverter();

		public bool ConvertToHtmlPageModel(SqliteDataReader reader, HtmlPage page)
		{
			while (reader.Read())
			{
				return SqliteDataToHtmlModel(reader, page);
			}

			return false;
		}

		protected bool SqliteDataToHtmlModel(SqliteDataReader reader, HtmlPage page)
		{
			page.Id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
			page.Content = reader.GetString(reader.GetOrdinal("content"));
			page.Created = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("created")));
			page.LastChanged = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("changed")));
			page.IsLocked = reader.GetBoolean(reader.GetOrdinal("locked"));

			var link = reader.GetString(reader.GetOrdinal("link"));
			var path = link.Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries);

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

		public IList<HtmlPage> ConvertToPageModels(SqliteDataReader reader)
		{
			var pages = new List<HtmlPage>();

			while (reader.Read())
			{
				var page = new HtmlPage();
				if (SqliteDataToHtmlModel(reader, page)) pages.Add(page);
			}

			return pages;
		}
	}
}
