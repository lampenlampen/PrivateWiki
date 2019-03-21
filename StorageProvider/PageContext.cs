using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NodaTime;

namespace StorageProvider
{
	[Obsolete("Use DataAccessLibrary.DataAccess instead.", true)]
	public class PageContext : DbContext
	{
		public DbSet<ContentPage> Pages { get; set; }
		public DbSet<Tag> Tags { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlite("Data Source=pages.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			var instantConverter = new ValueConverter<Instant?, long?>(
				v => InstantToLongConverter.ConvertToProvider(v),
				v => InstantToLongConverter.ConvertFromProvider(v));

			modelBuilder.Entity<ContentPage>()
				.Property(p => p.ExternalFileImportDate)
				.HasConversion(instantConverter);
		}
	}
}