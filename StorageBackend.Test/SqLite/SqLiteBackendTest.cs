using System;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using PrivateWiki.Models.Pages;
using PrivateWiki.UWP.StorageBackend;
using PrivateWiki.UWP.StorageBackend.SQLite;

namespace StorageBackend.Test.SqLite
{
	[TestClass]
	public class SqLiteBackendTest
	{
		private SqLiteBackend sqliteBackend;
		private IClock clock;

		[TestInitialize]
		public void Initialize()
		{
			File.Delete("test.db");
			clock = SystemClock.Instance;
			var storage = new SqLiteStorage("test");
			sqliteBackend = new SqLiteBackend(storage, clock);
			sqliteBackend.CreateTablesAsync().Wait();
		}

		/*[TestCleanup]
		public void DeleteTestDatabase()
		{
			File.Delete(sqliteBackend.Connection.DataSource);
		}*/

		[TestMethod]
		public void CreateTablesAsync()
		{
			var task = sqliteBackend.CreateTablesAsync();
			task.Wait();

			using var conn = sqliteBackend.Connection;
			conn.Open();

			var command = new SqliteCommand
			{
				Connection = conn,
				CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='markdown_pages';"
			};

			var command2 = new SqliteCommand
			{
				Connection = conn,
				CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='markdown_pages_history';"
			};

			var result = (string) command.ExecuteScalar();
			var result2 = (string) command2.ExecuteScalar();

			Assert.AreEqual("markdown_pages", result, "created table \"markdown_pages\" could not be found in db.");
			Assert.AreEqual("markdown_pages_history", result2, "created table \"markdown_pages_history\" could not be found in db.");
		}

		[TestMethod]
		public void ExistsTest()
		{
			var task = sqliteBackend.ExistsAsync();
			task.Wait();

			Assert.IsTrue(task.Result);
		}

		[TestMethod]
		public void ExistsNotTest()
		{
			var task = sqliteBackend.ExistsAsync();
			task.Wait();

			Assert.IsFalse(task.Result);
		}

		[TestMethod]
		public void InsertPageTest()
		{
			var page = new MarkdownPage(Guid.NewGuid(), "testlink", "test", clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
			var task = sqliteBackend.InsertMarkdownPageAsync(page);
			task.Wait();

			var result = task.Result;
		}

		[TestMethod]
		public void ContainsPageTest()
		{
			var page = new MarkdownPage(Guid.NewGuid(), "testlink5", "test", clock.GetCurrentInstant(), clock.GetCurrentInstant(), false);
			var task = sqliteBackend.InsertMarkdownPageAsync(page);
			task.Wait();

			page.Content = "test2";

			var task2 = sqliteBackend.UpdateMarkdownPage(page, PageAction.Edited);
			task2.Wait();

			page.Content = "test3";

			var task3 = sqliteBackend.UpdateMarkdownPage(page, PageAction.Edited);
			task3.Wait();

			Assert.IsTrue(task2.Result);

			var task4 = sqliteBackend.DeleteMarkdownPageAsync(page);
			task4.Wait();
		}
	}
}