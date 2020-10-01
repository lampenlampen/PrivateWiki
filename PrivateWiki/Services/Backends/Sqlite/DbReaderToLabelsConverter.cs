using System.Collections.Generic;
using System.Data.Common;
using PrivateWiki.Core;
using PrivateWiki.DataModels.Pages;

namespace PrivateWiki.Services.Backends.Sqlite
{
	public class DbReaderToLabelsConverter : IConverter<DbDataReader, IList<Label>>
	{
		private readonly IConverter<DbDataReader, Label> _converter;

		public DbReaderToLabelsConverter(IConverter<DbDataReader, Label> converter)
		{
			_converter = converter;
		}

		public IList<Label> Convert(DbDataReader reader)
		{
			var list = new List<Label>();

			while (reader.Read())
			{
				list.Add(_converter.Convert(reader));
			}

			return list;
		}
	}
}