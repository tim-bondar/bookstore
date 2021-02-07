using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DB.Abstraction;
using DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace DB.Repositories
{
    public class BooksRepository : IBooksRepository
    {
        private readonly BookstoreContext _context;

        public BooksRepository(BookstoreContext context)
        {
            context.Database.EnsureCreated();
            _context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            // TODO: implement pagination?
            return await _context.Books.AsNoTracking().ToListAsync();
        }

        public async Task<Book> GetById(Guid id)
        {
            return await GetBook(id);
        }

        public async Task Delete(Guid id)
        {
            var book = await GetBookWithImage(id);
            if (book != null)
            {
                _context.Images.Remove(book.CoverImage);
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Book> Update(Guid id, Book newBook, CoverImage coverImage)
        {
            // Preventing modification of different entity
            var book = await GetBookWithImage(id);

            book.Id = id;
            book.Author = newBook.Author;
            book.Title = newBook.Title;
            book.Description = newBook.Description;
            book.Price = newBook.Price;
            book.CoverImage.Content = coverImage.Content;
            book.CoverImage.ContentType = coverImage.ContentType;

            _context.Books.Attach(book).State = EntityState.Modified;
            _context.Images.Attach(book.CoverImage).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await GetBook(id);
        }

        public async Task<Book> Add(Book book, CoverImage coverImage)
        {
            book.Id = Guid.NewGuid();
            book.CoverImage = new CoverImage
            {
                Id = Guid.NewGuid(),
                Content = coverImage.Content,
                ContentType = coverImage.ContentType
            };

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return await GetBook(book.Id);
        }

        public async Task<CoverImage> GetImageByBookId(Guid bookId)
        {
            var book = await GetBookWithImage(bookId);
            return book?.CoverImage;
        }

        private async Task<Book> GetBook(Guid id)
        {
            return await _context.Books
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        private async Task<Book> GetBookWithImage(Guid id)
        {
            return await _context.Books
                .Include(x => x.CoverImage)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
