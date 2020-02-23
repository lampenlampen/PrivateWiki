using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using Models.Pages;
using NodaTime;

namespace StorageBackend.SQLite
{
	class SqliteDataReaderToHistoryPageConverter : SqliteDataReaderToPageConverter
	{
		public new static SqliteDataReaderToHistoryPageConverter Instance =>
			new SqliteDataReaderToHistoryPageConverter();

		public IEnumerable<GenericPageHistory> ConvertToHistoryPageModels(SqliteDataReader reader)
		{
			var pages = new List<GenericPageHistory>();

			while (reader.Read())
			{
				var page = new GenericPage();
				if (SqliteDataToPageModel(reader, page))
				{
					var action = (PageAction)reader.GetInt32(reader.GetOrdinal("action"));

					var pageHistory = new GenericPageHistory(page)
					{
						ValidFrom =
							Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_from"))),
						ValidTo = Instant.FromUnixTimeMilliseconds(reader.GetInt64(reader.GetOrdinal("valid_to"))),
						Action = (PageAction)reader.GetInt16(reader.GetOrdinal("action"))
					};

					pages.Add(pageHistory);
				}
			}

			return pages;
		}

	}
}
