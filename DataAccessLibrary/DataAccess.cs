using System;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using DataAccessLibrary.PageAST.Blocks;
using NodaTime;

namespace DataAccessLibrary
{
	public class DataAccess : IDataAccess
	{
		public SqliteConnection Db { get; }
		public IClock Clock { get; }


		public DataAccess(SqliteConnection db, IClock clock)
		{
			Db = db;
			Clock = clock;
		}

		public void InitializeDatabase()
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand(SQLiteHelper.PagesTable.createPageTableCommand, Db);
				var command2 = new SqliteCommand(SQLiteHelper.MarkdownBlockTable.createMarkdownBlockTableCommand, Db);

				command.ExecuteReader();
				command2.ExecuteReader();

				Db.Close();
			}
		}

		private bool CheckPageModelInvariant(PageModel page)
		{
			Debug.Assert(page.Id != null, "Id of page must be non null.");
			Debug.Assert(page.Content != null, "Content of page must be non null.");
			return true;
		}

		public void InsertPage(PageModel page)
		{
			CheckPageModelInvariant(page);

			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"INSERT INTO {SQLiteHelper.PagesTable.tableName} VALUES (@Id, @Link, @Content, @CreationTime, @ChangeTime, @IsFavorite, @IsLocked, @ExternalFileToken, @ExternalFileImportDate)"
				};

				command.Parameters.AddWithValue("@Id", page.Id.ToString());
				command.Parameters.AddWithValue("@Link", page.Link);
				command.Parameters.AddWithValue("@Content", page.Content);
				command.Parameters.AddWithValue("@CreationTime", Clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@ChangeTime", Clock.GetCurrentInstant().ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@IsFavorite", page.IsFavorite);
				command.Parameters.AddWithValue("@IsLocked", page.IsLocked);

				if (page.ExternalFileToken != null)
					command.Parameters.AddWithValue("@ExternalFileToken", page.ExternalFileToken);
				else command.Parameters.AddWithValue("@ExternalFileToken", DBNull.Value);

				if (page.ExternalFileImportDate != null)
				{
					var importDate = page.ExternalFileImportDate.Value;
					command.Parameters.AddWithValue("@ExternalFileImportDate",
						importDate.ToUnixTimeMilliseconds());
				}
				else
				{
					command.Parameters.AddWithValue("@ExternalFileImportDate",
						DBNull.Value);
				}

				command.ExecuteReader();
				Db.Close();
			}
		}

		public void InsertPages(IEnumerable<PageModel> pages)
		{
			foreach (var page in pages)
			{
				InsertPage(page);
			}
		}

		public List<PageModel> GetPages()
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand($"SELECT * FROM {SQLiteHelper.PagesTable.tableName}", Db);

				var query = command.ExecuteReader();

				var pages = SQLiteQueryToPageModelConverter.ConvertSQLiteQueryToPageModels(query);

				Db.Close();

				return pages;
			}
		}

		public PageModel GetPageOrNull(Guid id)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"SELECT * FROM {SQLiteHelper.PagesTable.tableName} WHERE {SQLiteHelper.PagesTable.col_id} = @Id"
				};

				command.Parameters.AddWithValue("@Id", id.ToString());

				var query = command.ExecuteReader();

				var pages = SQLiteQueryToPageModelConverter.ConvertSQLiteQueryToPageModels(query);

				if (pages.Count == 0)
				{
					// TODO No Page found
					return null;
				}

				if (pages.Count == 1)
				{
					// Page found
					return pages.First();
				}

				// Error: More than one page with the same id.
				throw new Exception("Too many pages");
			}
		}

		public PageModel GetPageOrNull(string link)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"SELECT * FROM {SQLiteHelper.PagesTable.tableName} WHERE {SQLiteHelper.PagesTable.col_link} = @Link"
				};

				command.Parameters.AddWithValue("@Link", link);

				var query = command.ExecuteReader();

				var pages = SQLiteQueryToPageModelConverter.ConvertSQLiteQueryToPageModels(query);

				if (pages.Count == 0)
				{
					// TODO No Page found
					return null;
				}

				if (pages.Count == 1)
				{
					// Page found
					return pages.First();
				}

				// Error: More than one page with the same id.
				throw new Exception("Too many pages");
			}
		}

		public void UpdatePage(PageModel page)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					CommandText = $"UPDATE {SQLiteHelper.PagesTable.tableName} SET " +
					              $"{SQLiteHelper.PagesTable.col_content} = @Content, " +
					              $"{SQLiteHelper.PagesTable.col_creationDate} = @CreationDate, " +
					              $"{SQLiteHelper.PagesTable.col_lastChangeDate} = {Clock.GetCurrentInstant().ToUnixTimeMilliseconds()}, " +
					              $"{SQLiteHelper.PagesTable.col_isFavorite} = @IsFavorite, " +
					              $"{SQLiteHelper.PagesTable.col_isLocked} = @IsLocked, " +
					              $"{SQLiteHelper.PagesTable.col_externalFileToken} = @ExternalFileToken, " +
					              $"{SQLiteHelper.PagesTable.col_externalFileImportDate} = @ExternalFileImportDate " +
					              $"WHERE {SQLiteHelper.PagesTable.col_id} = @Id",
					Connection = Db
				};
				command.Parameters.AddWithValue("@Content", page.Content);
				command.Parameters.AddWithValue("@CreationDate", page.CreationTime.ToUnixTimeMilliseconds());
				command.Parameters.AddWithValue("@IsFavorite", page.IsFavorite);
				command.Parameters.AddWithValue("@IsLocked", page.IsLocked);
				command.Parameters.AddWithValue("@Id", page.Id.ToString());

				if (page.ExternalFileToken != null)
					command.Parameters.AddWithValue("@ExternalFileToken", page.ExternalFileToken);
				else command.Parameters.AddWithValue("@ExternalFileToken", DBNull.Value);

				if (page.ExternalFileImportDate != null)
				{
					var importDate = page.ExternalFileImportDate.Value;
					command.Parameters.AddWithValue("@ExternalFileImportDate",
						importDate.ToUnixTimeMilliseconds());
				}
				else
				{
					command.Parameters.AddWithValue("@ExternalFileImportDate",
						DBNull.Value);
				}

				command.ExecuteReader();

				Db.Close();
			}
		}

		public void UpdateContent(PageModel page)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					CommandText =
						$"UPDATE {SQLiteHelper.PagesTable.tableName} SET {SQLiteHelper.PagesTable.col_content} = @Content WHERE {SQLiteHelper.PagesTable.col_id} = @Id",
					Connection = Db
				};
				command.Parameters.AddWithValue("@Content", page.Content);
				command.Parameters.AddWithValue("@Id", page.Id.ToString());

				command.ExecuteReader();

				Db.Close();
			}
		}

		public bool ContainsPage(PageModel page)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"SELECT * FROM {SQLiteHelper.PagesTable.tableName} WHERE {SQLiteHelper.PagesTable.col_id} = @Id OR {SQLiteHelper.PagesTable.col_link} = @Link"
				};

				command.Parameters.AddWithValue("@Id", page.Id.ToString());
				command.Parameters.AddWithValue("@Link", page.Link);

				var query = command.ExecuteReader();

				var count = 0;

				while (query.Read())
				{
					count++;
				}

				Db.Close();

				return count != 0;
			}
		}

		public bool ContainsPage(Guid id)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"SELECT * FROM {SQLiteHelper.PagesTable.tableName} WHERE {SQLiteHelper.PagesTable.col_id} = @Id"
				};

				command.Parameters.AddWithValue("@Id", id.ToString());

				var query = command.ExecuteReader();

				var count = 0;

				while (query.Read())
				{
					count++;
				}

				Db.Close();

				return count != 0;
			}
		}

		public bool ContainsPage(string link)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"SELECT EXISTS (SELECT * FROM {SQLiteHelper.PagesTable.tableName} WHERE {SQLiteHelper.PagesTable.col_link} = @Link)"
				};

				command.Parameters.AddWithValue("@Link", link);

				var query = command.ExecuteReader();

				var count = 0;

				while (query.Read())
				{
					count++;
				}

				Db.Close();

				return count != 0;
			}
		}

		public bool DeletePage(PageModel page)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"DELETE FROM {SQLiteHelper.PagesTable.tableName} WHERE {SQLiteHelper.PagesTable.col_id} = @Id"
				};
				command.Parameters.AddWithValue("@Id", page.Id.ToString());

				command.ExecuteReader();

				Db.Close();

				return true;
			}
		}

		public void InsertMarkdownBlock(MarkdownBlock block)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"INSERT INTO {SQLiteHelper.MarkdownBlockTable.MarkdownBlockTableName} VALUES (@Id, @Source)"
				};

				command.Parameters.AddWithValue("@Id", block.Id.ToString());
				command.Parameters.AddWithValue("@Source", block.Source);

				command.ExecuteReader();

				Db.Close();
			}
		}

		public MarkdownBlock GetMarkdownBlockOrNull(Guid id)
		{
			using (Db)
			{
				Db.Open();

				var command = new SqliteCommand
				{
					Connection = Db,
					CommandText =
						$"SELECT * FROM {SQLiteHelper.MarkdownBlockTable.MarkdownBlockTableName} WHERE {SQLiteHelper.MarkdownBlockTable.col_Id} = @Id"
				};

				command.Parameters.AddWithValue("@Id", id.ToString());

				var query = command.ExecuteReader();

				var blocks = new List<MarkdownBlock>();

				while (query.Read())
				{
					var source = query.GetString(query.GetOrdinal(SQLiteHelper.MarkdownBlockTable.col_Source));

					var block = new MarkdownBlock(id, Markdig.Parser.ParseToMarkdownDocument(source), source);
					
					blocks.Add(block);
				}

				if (blocks.Count > 1) throw new Exception("Too many MarkdownBlocks");

				return blocks.First();
			}
		}
	}
}