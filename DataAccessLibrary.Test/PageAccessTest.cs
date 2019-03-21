using System;
using System.Data.Common;
using System.IO;
using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;

namespace DataAccessLibrary.Test
{
	[TestClass]
	public class PageAccessTest
	{
		private SqliteConnection _db;
		private IClock _clock;

		[TestInitialize]
		public void CreateDBConnection()
		{
			_db = SQLiteHelper.GetDBConnection();
			_clock = SystemClock.Instance;
		}

		[TestCleanup]
		public void DeleteTestDatabase()
		{
			File.Delete(_db.DataSource);
		}

		[TestMethod]
		public void CreateDatabaseTest()
		{
			
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();
			var path = Path.GetFullPath(_db.DataSource);

			Assert.IsTrue(File.Exists(path), $"DB was not created or is not at the location: {path}");
		}

		[TestMethod]
		public void InsertPageTest()
		{
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1", _clock);
			var page2 = new PageModel(Guid.NewGuid(), "page2", "# Page 2", _clock);

			db.InsertPage(page1);
			db.InsertPage(page2);
			var pages = db.GetPages();

			var dbPage1 = pages.Find(p => p.Id.Equals(page1.Id));
			var dbPage2 = pages.Find(p => p.Id.Equals(page2.Id));

			Assert.IsTrue(dbPage1 != null, "Page1 was not saved to the db.", pages, new[] { page1 });
			Assert.IsTrue(dbPage2 != null, "Page2 was not saved to the db.", pages, new[] { page2 });
		}

		[TestMethod]
		public void InsertPagesTest()
		{
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var page1 = new PageModel(Guid.NewGuid(),"page1", "# Page 1", _clock);
			var page2 = new PageModel(Guid.NewGuid(), "page2", "# Page 2", _clock);

			db.InsertPages(new[] {page1, page2});
			var pages = db.GetPages();

			var dbPage1 = pages.Find(p => p.Id.Equals(page1.Id));
			var dbPage2 = pages.Find(p => p.Id.Equals(page2.Id));

			Assert.IsTrue(dbPage1 != null, "Page1 was not saved to the db.");
			Assert.IsTrue(dbPage2 != null, "Page2 was not saved to the db.");
		}

		[TestMethod]
		public void GetPageTest()
		{
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1", _clock);

			db.InsertPage(page1);

			var dbPage = db.GetPageOrNull(page1.Id);

			Assert.IsTrue(dbPage.Id.Equals(page1.Id), "Page could not be retrieved from db.", page1, dbPage);
		}

		[TestMethod]
		public void UpdateContentTest()
		{
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var page1 = new PageModel(Guid.NewGuid(), "page1","# Page 1", _clock);

			db.InsertPage(page1);

			page1.Content = "# Page2 Updated Content";

			db.UpdateContent(page1);

			var dbPage = db.GetPageOrNull(page1.Id);

			Assert.IsTrue(dbPage.Id.Equals(page1.Id), "Page Update was not successful.");
			Assert.IsTrue(dbPage.Content.Equals(page1.Content), "Page Update was not successful.");
		}
		
		[TestMethod]
		public void UpdatePageTest()
		{
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1", _clock);

			db.InsertPage(page1);

			page1.Content = "# Page2 Updated Content";

			db.UpdatePage(page1);

			var dbPage = db.GetPageOrNull(page1.Id);

			Assert.IsTrue(dbPage.Id.Equals(page1.Id), "Page Update was not successful.");
			Assert.IsTrue(dbPage.Content.Equals(page1.Content), "Page Update was not successful.");
		}

		[TestMethod]
		public void DeletePageTest()
		{
			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();
			
			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1",_clock);
			
			db.InsertPage(page1);
			
			Assert.IsTrue(db.ContainsPage(page1), "Page was not saved in db.");

			db.DeletePage(page1);
			
			Assert.IsFalse(db.ContainsPage(page1), "Page was not deleted from db.");
		}
	}
}
