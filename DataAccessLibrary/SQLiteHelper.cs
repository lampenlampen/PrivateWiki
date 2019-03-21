using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace DataAccessLibrary
{
	public static class SQLiteHelper
	{
		internal const string dbName = "wiki";
		internal const string tableName = "Pages";
		internal const string col_id = "id";
		internal const string col_link = "link";
		internal const string col_content = "content";
		internal const string col_creationDate = "date_created";
		internal const string col_lastChangeDate = "date_last_changed";
		internal const string col_isFavorite = "is_favorite";
		internal const string col_isLocked = "is_locked";
		internal const string col_externalFileToken = "external_file_token";
		internal const string col_externalFileImportDate = "external_file_import_date";

		public static readonly string createPageTableCommand = $"CREATE TABLE IF NOT EXISTS {tableName} (" +
		                                                    $"{col_id} STRING PRIMARY KEY," +
		                                                    $"{col_link} STRING UNIQUE, " +
		                                                    $"{col_content} STRING," +
		                                                    $"{col_creationDate} INTEGER," +
		                                                    $"{col_lastChangeDate} INTEGER," +
		                                                    $"{col_isFavorite} INTEGER," +
		                                                    $"{col_isLocked} INTEGER," +
		                                                    $"{col_externalFileToken} STRING," +
		                                                    $"{col_externalFileImportDate} INTEGER" +
		                                                    $")";

		public static SqliteConnection GetDBConnection()
		{
			var db = new SqliteConnection($"FILENAME={SQLiteHelper.dbName}.db");

			return db;
		}
	}
}
