using AutoMapper;
using Core.Models;
using DB.Entities;

namespace Features
{
    // Used via reflection in Startup.cs
    public class MapProfile : Profile
    {
        public MapProfile()
        {
            CreateMap<AddBookModel, Book>();
            CreateMap<Book, BookModel>();
            CreateMap<BookModel, Book>();
        }
    }
}
