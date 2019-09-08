using System;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Storage;
using NodaTime;
using StorageBackend.SQLite;

namespace StorageBackend.Test.SqLite
{
	[TestClass]
	public class SqLiteBackendTest
	{
		[TestMethod]
		public void CreateTablesAsync()
		{
			var storage = new SqLiteStorage("test");

			var sqliteBackend = new SqLiteBackend(storage, SystemClock.Instance);

			var task = sqliteBackend.CreateTablesAsync();
			task.Wait();

			using var conn = new SqliteConnection($"FILENAME={storage.Filename}.db");
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
			var storage = new SqLiteStorage("test");

			var sqliteBackend = new SqLiteBackend(storage, SystemClock.Instance);

			var task =  sqliteBackend.ExistsAsync();
			task.Wait();
			
			Assert.IsTrue(task.Result);
		}

		[TestMethod]
		public void ExistsNotTest()
		{
			var storage = new SqLiteStorage("test2");

			var sqliteBackend = new SqLiteBackend(storage, SystemClock.Instance);

			var task = sqliteBackend.ExistsAsync();
			task.Wait();
			
			Assert.IsFalse(task.Result);
		}
	}
}