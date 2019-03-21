using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using PrivateWiki.Data.DataAccess;
using StorageProvider;

namespace PrivateWiki.Data
{
	[Obsolete("Use DataAccessImpl instead", true)]
	internal class ContentPageProvider : DataAccessImpl
	{
		private IClock Clock { get; set; }

		public ContentPageProvider(IClock clock)
		{
			Clock = clock;
		}

		public void InitializeDatabase()
		{
			using (var db = new PageContext())
			{
				db.Database.Migrate();
			}
		}

		[NotNull]
		public ContentPage GetPage([NotNull] string id)
		{
			using (var db = new PageContext())
			{
				return db.Pages.Single(p => p.Id.Equals(id));
			}
		}

		[NotNull]
		public List<ContentPage> GetAllPages()
		{
			using (var db = new PageContext())
			{
				return db.Pages.ToList();
			}
		}

		public bool InsertPage([NotNull] ContentPage page)
		{
			page.CreationTime = Clock.GetCurrentInstant();
			page.ChangeTime = Clock.GetCurrentInstant();

			using (var db = new PageContext())
			{
				db.Pages.Add(page);
				db.SaveChanges();
				return true;
			}
		}

		public bool UpdatePage([NotNull] ContentPage page)
		{
			page.ChangeTime = Clock.GetCurrentInstant();


			using (var db = new PageContext())
			{
				db.Pages.Update(page);
				db.SaveChanges();
				return true;
			}
		}

		public bool DeletePage([NotNull] ContentPage page)
		{
			using (var db = new PageContext())
			{
				db.Pages.Remove(page);
				db.SaveChanges();
				return true;
			}
		}

		public bool ContainsPage([NotNull] ContentPage page)
		{
			using (var db = new PageContext())
			{
				return db.Pages.Contains(page);
			}
		}

		public bool ContainsPage([NotNull] string id)
		{
			return ContainsPage(ContentPage.Create(id));
		}
	}
}