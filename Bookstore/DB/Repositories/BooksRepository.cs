using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Exceptions;
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
            _context = context;
        }

        public async Task<List<Book>> GetAll()
        {
            // TODO: implement pagination
            return await _context.Books.ToListAsync();
        }

        public async Task<Book> GetById(Guid id)
        {
            return await GetBook(id);
        }

        public async Task Delete(Guid id)
        {
            var book = await GetBook(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book> Update(Guid id, Book book)
        {
            var oldBook = await GetBook(id);

            // Preventing modification of different entity
            book.Id = oldBook.Id;

            _context.Books.Attach(book).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return await GetBook(id);
        }

        public async Task<Book> Add(Book book)
        {
            book.Id = Guid.NewGuid();

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return await GetBook(book.Id);
        }

        private async Task<Book> GetBook(Guid id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                throw new BookNotFoundException($"Book with ID {id} was not found.");
            }

            return book;
        }
    }
}
