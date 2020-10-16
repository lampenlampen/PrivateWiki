using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using FluentResults;
using PrivateWiki.Core;
using PrivateWiki.DataModels.Errors;
using PrivateWiki.DataModels.Pages;
using PrivateWiki.Services.AppSettingsService.BackendSettings;
using PrivateWiki.Services.StorageServices.Sql;
using PrivateWiki.Services.StorageServices.Sql.Sqlite;
using PrivateWiki.Utilities;

namespace PrivateWiki.Services.Backends.Sqlite
{
	public class LabelSqliteBackend : ILabelBackend
	{
		private readonly ISqliteStorage _sqliteStorage;
		private readonly IConverter<DbDataReader, Label> _toLabelConverter;
		private readonly IConverter<DbDataReader, IList<Label>> _toLabelsConverter;

		private readonly Task _initTask;

		public LabelSqliteBackend(IBackendSettingsService backendSettings, IConverter<DbDataReader, Label> converter, IConverter<DbDataReader, IList<Label>> toLabelsConverter)
		{
			_toLabelConverter = converter;
			_toLabelsConverter = toLabelsConverter;
			_sqliteStorage = new SqliteDatabase(new SqliteStorageOptions {Path = backendSettings.GetSqliteBackendPath()});

			_initTask = CreateTable();
		}

		private Task CreateTable()
		{
			const string sql = "CREATE TABLE IF NOT EXISTS labels (id TEXT PRIMARY KEY, key TEXT, value TEXT, description TEXT, color TEXT);";

			var command = new SqlCommand(sql, new List<KeyValuePair<string, string>>());

			return _sqliteStorage.ExecuteNonQueryAsync(command);
		}

		public async Task InsertLabelAsync(Label label)
		{
			await _initTask;

			const string sql = "INSERT INTO 'labels' (id, key, value, description, color) VALUES (@id, @key, @value, @description, @color)";
			var parameters = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@id", label.Id.ToString()),
				new KeyValuePair<string, string>("@key", label.Key),
				new KeyValuePair<string, string>("@value", label.Value),
				new KeyValuePair<string, string>("@description", label.Description),
				new KeyValuePair<string, string>("@color", label.Color.ToHexColor())
			};

			var command = new SqlCommand(sql, parameters);

			await _sqliteStorage.ExecuteNonQueryAsync(command);
		}

		public async Task<Result<Label>> GetLabelAsync(Guid id)
		{
			await _initTask;

			const string sql = "SELECT * FROM labels WHERE id == @id";
			var parameter = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@id", id.ToString())
			};

			var command = new SqlCommand(sql, parameter);

			Label label;
			try
			{
				label = await _sqliteStorage.ExecuteReaderAsync(command, _toLabelConverter);
			}
			catch (Exception e)
			{
				return Result.Fail(new NotFoundError().CausedBy(e));
			}

			return Result.Ok(label);
		}

		public async Task<IEnumerable<Label>> GetAllLabelsAsync()
		{
			await _initTask;

			const string sql = "SELECT * FROM labels";

			var command = new SqlCommand(sql, new List<KeyValuePair<string, string>>());

			var label = await _sqliteStorage.ExecuteReaderAsync(command, _toLabelsConverter);

			return label;
		}

		public async Task DeleteLabelAsync(Guid id)
		{
			await _initTask;

			const string sql = "DELETE FROM labels WHERE id == @id";

			var parameter = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@id", id.ToString())
			};

			var command = new SqlCommand(sql, parameter);

			await _sqliteStorage.ExecuteNonQueryAsync(command);
		}

		public async Task UpdateLabelAsync(Label label)
		{
			await _initTask;

			const string sql = "UPDATE 'labels' SET key = @key, value = @value, description = @description, color = @color WHERE id = @id";

			var parameters = new List<KeyValuePair<string, string>>
			{
				new KeyValuePair<string, string>("@id", label.Id.ToString()),
				new KeyValuePair<string, string>("@key", label.Key),
				new KeyValuePair<string, string>("@value", label.Value),
				new KeyValuePair<string, string>("@description", label.Description),
				new KeyValuePair<string, string>("@color", label.Color.ToHexColor())
			};

			var command = new SqlCommand(sql, parameters);

			await _sqliteStorage.ExecuteNonQueryAsync(command);
		}
	}
}