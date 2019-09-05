using Microsoft.Data.Sqlite;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using System;
using System.IO;
using StorageBackend.PageAST;
using StorageBackend.PageAST.Blocks;

namespace StorageBackend.Test
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

			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1", _clock);
			var page2 = new PageModel(Guid.NewGuid(), "page2", "# Page 2", _clock);

			db.InsertPages(new[] { page1, page2 });
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

			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1", _clock);

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

			var page1 = new PageModel(Guid.NewGuid(), "page1", "# Page 1", _clock);

			db.InsertPage(page1);

			Assert.IsTrue(db.ContainsPage(page1), "Page was not saved in db.");

			db.DeletePage(page1);

			Assert.IsFalse(db.ContainsPage(page1), "Page was not deleted from db.");
		}

		[TestMethod]
		public void InsertMarkdownBlockTest()
		{
			var markdown =
				"# Welcome to your Private Wiki\n\n## Get Started\n\nTo learn more about the syntax have a lock in the [Syntax](:syntax) page.\n\nTo view a preview article follow this [link](:test)";

			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var block1 = new MarkdownBlock(Guid.NewGuid(), Markdig.Parser.ParseToMarkdownDocument(markdown), markdown);

			db.InsertMarkdownBlock(block1);
		}

		[TestMethod]
		public void GetMarkdownBlockOrNullTest()
		{
			string markdown =
				"# Welcome to your Private Wiki\n\n## Get Started\n\nTo learn more about the syntax have a lock in the [Syntax](:syntax) page.\n\nTo view a preview article follow this [link](:test)";

			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			var block = new MarkdownBlock(Guid.NewGuid(), Markdig.Parser.ParseToMarkdownDocument(markdown), markdown);

			db.InsertMarkdownBlock(block);

			var output = db.GetMarkdownBlockOrNull(block.Id);

			Assert.AreEqual(block.Id, output.Id);
			Assert.AreEqual(block.Source, output.Source);
		}

		[TestMethod]
		public void InsertDocument()
		{
			var document = new Document(
				Guid.NewGuid(),
				new TitleBlock("title1", "title1"),
				_clock.GetCurrentInstant(),
				_clock.GetCurrentInstant(),
				"Document1",
				"2;8;9;5");

			var db = new DataAccess(_db, _clock);
			db.InitializeDatabase();

			db.InsertDocument(document);

			var output = db.GetDocument(document.Id);

			Assert.AreEqual(document.Id, output.Id);

			// TODO Check Equals
		}
	}
}