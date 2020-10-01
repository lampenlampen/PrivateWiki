using System;
using System.Collections.Generic;
using System.Data.Common;
using PrivateWiki.Core;

namespace PrivateWiki.Services.Backends.Sqlite
{
	public class DbReaderToLabelIdsConverter : IConverter<DbDataReader, IEnumerable<Guid>>
	{
		public IEnumerable<Guid> Convert(DbDataReader reader)
		{
			var list = new List<Guid>();

			while (reader.Read())
			{
				var id = reader.GetGuid(reader.GetOrdinal("label_id"));

				list.Add(id);
			}

			return list;
		}
	}
}