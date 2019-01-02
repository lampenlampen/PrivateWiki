using Microsoft.EntityFrameworkCore;

namespace StorageProvider
{
	public class PageContext : DbContext
	{
		public DbSet<ContentPage> Pages { get; set; }
		public DbSet<Tag> Tags { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=pages.db");
		}
	}
}