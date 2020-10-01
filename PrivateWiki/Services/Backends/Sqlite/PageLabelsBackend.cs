using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using PrivateWiki.Core;
using PrivateWiki.Services.AppSettingsService.BackendSettings;
using PrivateWiki.Services.StorageServices.Sql;
using PrivateWiki.Services.StorageServices.Sql.Sqlite;

namespace PrivateWiki.Services.Backends.Sqlite
{
	public class PageLabelsBackend : IPageLabelsBackend
	{
		private readonly ISqliteStorage _sqliteStorage;
		private readonly IConverter<DbDataReader, IEnumerable<Guid>> _toLabelsIdConverter;

		private readonly Task _initTask;

		public PageLabelsBackend(IBackendSettingsService backendSettings, IConverter<DbDataReader, IEnumerable<Guid>> toLabelsIdConverter)
		{
			_sqliteStorage = new SqliteDatabase(new SqliteStorageOptions {Path = backendSettings.GetSqliteBackendPath()});
			_toLabelsIdConverter = toLabelsIdConverter;

			_initTask = CreateTable();
		}

		private Task CreateTable()
		{
			const string sql =
				"CREATE TABLE IF NOT EXISTS page_labels (page_id TEXT,label_id TEXT,PRIMARY KEY (page_id, label_id),FOREIGN KEY (page_id) REFERENCES pages(id) on delete CASCADE,FOREIGN KEY (label_id) REFERENCES labels(id) on delete CASCADE);";

			var command = new SqlCommand(sql, new List<KeyValuePair<string, string>>());

			return _sqliteStorage.ExecuteNonQueryAsync(command);
		}

		public async Task<IEnumerable<Guid>> GetLabelIdsForPageId(Guid pageId)
		{
			await _initTask;

			const string sql = "SELECT label_id FROM page_labels WHERE page_id = @pageId";

			var parameter = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@pageId", pageId.ToString())
			};

			var command = new SqlCommand(sql, parameter);

			var result = await _sqliteStorage.ExecuteReaderAsync(command, _toLabelsIdConverter);

			return result;
		}
	}
}