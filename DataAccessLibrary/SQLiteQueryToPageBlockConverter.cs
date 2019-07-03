using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLibrary.PageAST;
using Microsoft.Data.Sqlite;
using NodaTime;

namespace DataAccessLibrary
{
	public class SQLiteQueryToPageBlockConverter
	{
		public IEnumerable<(Document, IEnumerable<Guid>)> ConvertToDocumentBlock(SqliteDataReader query)
		{
			var list = new List<(Document, IEnumerable<Guid>)>();

			while (query.Read())
			{
				
				
				var document = new Document(
					query.GetGuid(query.GetOrdinal(SQLiteHelper.DocumentTable.col_Id)),
					null,
					Instant.FromUnixTimeMilliseconds(query.GetInt64(query.GetOrdinal(SQLiteHelper.DocumentTable.col_CreationDate))),
					Instant.FromUnixTimeMilliseconds(query.GetInt64(query.GetOrdinal(SQLiteHelper.DocumentTable.col_ModifyDate))),
					query.GetString(query.GetOrdinal(SQLiteHelper.DocumentTable.col_Link)),
					null);
				
				// TODO Blocks
				var blocks = query.GetString(query.GetOrdinal(SQLiteHelper.DocumentTable.col_content)).Split(';')
					.Select(Guid.Parse);
				
				list.Add((document, blocks));
			}

			return list;
		}
	}
}