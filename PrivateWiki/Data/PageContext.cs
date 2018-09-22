using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateWiki.Data
{
    class PageContext : DbContext
    {
        public DbSet<PageModel> Pages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=pages.db");
            base.OnConfiguring(optionsBuilder);
        }
    }

    class PageModel
    {
        public string id { get; set; }
        public string markdown { get; set; }
    }
}
