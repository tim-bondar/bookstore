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
        public DbSet<CoverImage> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Book>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<CoverImage>()
                .HasKey(x => x.Id);

            modelBuilder.Entity<Book>()
                .HasOne<CoverImage>();
        }
    }
}
