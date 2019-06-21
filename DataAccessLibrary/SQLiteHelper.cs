using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Data.Sqlite;

namespace DataAccessLibrary
{
	public static class SQLiteHelper
	{
		internal const string dbName = "wiki";

		public static class PagesTable
		{
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

			internal static readonly string CreatePageTableCommand = $"CREATE TABLE IF NOT EXISTS {tableName} (" +
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
		}

		public static class MarkdownBlockTable
		{
			internal const string MarkdownBlockTableName = "markdown_block_table";
			internal const string col_Id = "id";
			internal const string col_Source = "source";

			internal static readonly string CreateMarkdownBlockTableCommand =
				$"CREATE TABLE IF NOT EXISTS {MarkdownBlockTableName} (" +
				$"{col_Id} STRING PRIMARY KEY, " +
				$"{col_Source} STRING" +
				$")";
		}

		public static class CodeBlockTable
		{
			internal const string CodeBlockTableName = "code_block_table";
			internal const string col_Id = "id";
			internal const string col_Code = "code";
			internal const string col_LanugageCode = "language_code";

			internal static readonly string CreateCodeBlockTableCommand =
				$"CREATE TABLE IF NOT EXISTS {CodeBlockTableName} (" +
				$"{col_Id} STRING PRIMARY KEY, " +
				$"{col_Code} STRING, " +
				$"{col_LanugageCode} STRING" +
				$")";
		}

		public static class DocumentTable
		{
			internal const string DocumentTableName = "document_table";
			internal const string col_Id = "id";
			internal const string col_CreationDate = "create_date";
			internal const string col_ModifyDate = "modify_date";
			internal const string col_Link = "link";
			internal const string col_Title = "title";

			internal static readonly string CreateDocumentTableCommand =
				$"CREATE TABLE IF NOT EXISTS {DocumentTableName} (" +
				$"{col_Id} STRING PRIMARY KEY, " +
				$"{col_CreationDate} INTEGER, " +
				$"{col_ModifyDate} INTEGER, " +
				$"{col_Link} STRING" +
				$"{col_Title} STRING" +
				$")";
		}

		public static class BlockOrderTable
		{
			internal const string BlockOrderTableName = "block_order_table";
			internal const string col_vorgaenger = "vorgaenger_id";
			internal const string col_nachfolger = "nachfolger_id";
			internal const string col_document_id = "document_id";

			internal static readonly string CreateBlockOrderTableCommand =
				$"CREATE TABLE IF NOT EXISTS {BlockOrderTableName} (" +
				$"{col_vorgaenger} STRING, " +
				$"{col_nachfolger} STRING, " +
				$"{col_document_id} STRING, " +
				$"PRIMARY KEY ({col_vorgaenger}, {col_vorgaenger}, {col_document_id})," +
				$"FOREIGN KEY ({col_vorgaenger}) REFERENCES {DocumentTable.DocumentTableName} ({DocumentTable.col_Id})";
		}


		public static SqliteConnection GetDBConnection()
		{
			var db = new SqliteConnection($"FILENAME={dbName}.db");

			return db;
		}
	}
}