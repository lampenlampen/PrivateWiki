using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using StorageProvider;

namespace PrivateWiki.Data
{
    class ContentPageProvider : IPageAccess
    {

        public void InitDatabase()
        {
            using (var db = new PageContext())
            {
                db.Database.Migrate();
            }
        }

        public ContentPage GetContentPage(string id)
        {
            using (var db = new PageContext())
            {
                return db.Pages.Single(p => p.Id.Equals(id));
            }
        }

        public bool InsertContentPage(ContentPage page)
        {
            using (var db = new PageContext())
            {
                db.Pages.Add(page);
                db.SaveChanges();
                return true;
            }
        }

        public bool UpdateContentPage(ContentPage page)
        {
            using (var db = new PageContext())
            {
                db.Pages.Update(page);
                db.SaveChanges();
                return true;
            }
        }
    }
}
