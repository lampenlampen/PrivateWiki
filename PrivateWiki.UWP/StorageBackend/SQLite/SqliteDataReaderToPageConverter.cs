using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using NodaTime;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.UWP.StorageBackend.SQLite
{
	class SqliteDataReaderToPageConverter
	{
		public static SqliteDataReaderToPageConverter Instance = new SqliteDataReaderToPageConverter();

		public bool ConvertToPageModel(SqliteDataReader reader, GenericPage page)
		{
			while (reader.Read())
			{
				return SqliteDataToPageModel(reader, page);
			}

			return false;
		}

		protected bool SqliteDataToPageModel(SqliteDataReader reader, GenericPage page)
		{
			page.Id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
			page.Link = reader.GetString(reader.GetOrdinal("link"));
			page.Content = reader.GetString(reader.GetOrdinal("content"));
			page.Created = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("created")));
			page.LastChanged = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("changed")));
			page.IsLocked = reader.GetBoolean(reader.GetOrdinal("locked"));
			//page.ContentType = reader.GetString(reader.GetOrdinal("contentType"));
			page.ContentType = ContentType.Parse(reader.GetString(reader.GetOrdinal("contentType")));

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


		public IList<GenericPage> ConvertToPageModels(SqliteDataReader reader)
		{
			var pages = new List<GenericPage>();

			while (reader.Read())
			{
				var page = new GenericPage();
				if (SqliteDataToPageModel(reader, page)) pages.Add(page);
			}

			return pages;
		}
	}
}