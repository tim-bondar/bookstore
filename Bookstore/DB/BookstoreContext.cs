using System;
using System.IO;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .HasKey(x => x.Id);

            // Initial seeding
            modelBuilder.Entity<Book>()
                .HasData(
                new Book
                {
                    Id = Guid.Parse("b8c56d97-ed68-4671-a503-21bda97682f3"),
                    Title = "Book Number One",
                    Description = "Very informative description",
                    Author = "T. Tickle",
                    Price = 13.69m,
                    ImageContentType = "image/jpg",
                    ImageContent = File.ReadAllBytes("default.jpg")
                },
                new Book
                {
                    Id = Guid.Parse("93c3a6c5-dfde-4dc6-af6f-9a3c23be5ea8"),
                    Title = "Book Number Two",
                    Description = "Very very informative description",
                    Author = "L. Baals",
                    Price = 123.45m,
                    ImageContentType = "image/jpg",
                    ImageContent = File.ReadAllBytes("default.jpg")
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}
