using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DB.Entities;

namespace DB.Abstraction
{
    public interface IBooksRepository
    {
        Task<List<Book>> GetAll();
        Task<Book> GetById(Guid id);
        Task Delete(Guid id);
        Task<Book> Update(Guid id, Book newBook, CoverImage coverImage);
        Task<Book> Add(Book book, CoverImage coverImage);
        Task<CoverImage> GetImageByBookId(Guid bookId);
    }
}
