using System;
using System.Data.Common;
using PrivateWiki.Core;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Utilities;

namespace PrivateWiki.Services.Backends.Sqlite
{
	public class DbReaderToLabelConverter : IConverter<DbDataReader, Label>
	{
		public Label Convert(DbDataReader reader)
		{
			var id = Guid.Parse(reader.GetString(reader.GetOrdinal("id")));
			var key = reader.GetString(reader.GetOrdinal("key"));
			var value = reader.GetString(reader.GetOrdinal("value"));
			var description = reader.GetString(reader.GetOrdinal("description"));
			var color = reader.GetString(reader.GetOrdinal("color")).HexToColor();

			var label = new Label(id, key, value, description, color);

			return label;
		}
	}
}