using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using StorageProvider;
using LanguageExt;
using static LanguageExt.Prelude;

namespace PrivateWiki.Data
{
    internal class ContentPageProvider : IPageAccess
    {
        public void InitDatabase()
        {
            using (var db = new PageContext())
            {
                db.Database.Migrate();
            }
        }

        [NotNull]
        public ContentPage GetContentPage([NotNull] string id)
        {
            using (var db = new PageContext())
            {
                return db.Pages.Single(p => p.Id.Equals(id));
            }
        }

        [NotNull]
        public List<ContentPage> GetAllContentPages()
        {
            using (var db = new PageContext())
            {
                return db.Pages.ToList();
            }
        }

        public bool InsertContentPage([NotNull] ContentPage page)
        {
            page.CreationTime = new DateTimeOffset();
            page.ChangeTime = new DateTimeOffset();

            using (var db = new PageContext())
            {
                db.Pages.Add(page);
                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateContentPage([NotNull] ContentPage page)
        {
            page.ChangeTime = DateTimeOffset.Now;
            

            using (var db = new PageContext())
            {
                db.Pages.Update(page);
                db.SaveChanges();
                return true;
            }
        }

        public bool DeleteContentPage([NotNull] ContentPage page)
        {
            using (var db = new PageContext())
            {
                db.Pages.Remove(page);
                db.SaveChanges();
                return true;
            }
        }

        public bool ContainsContentPage([NotNull] ContentPage page)
        {
            using (var db = new PageContext())
            {
                return db.Pages.Contains(page);
            }
        }

        public bool ContainsContentPage([NotNull] string id)
        {
            return ContainsContentPage(ContentPage.Create(id));
        }
    }
}