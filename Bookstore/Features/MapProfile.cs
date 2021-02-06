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
            // TODO: implement fields mapping
            CreateMap<Book, BookModel>();

            // TODO: implement fields mapping
            CreateMap<BookModel, Book>();

            // TODO: implement fields mapping
            CreateMap<CoverImage, CoverImageModel>();

            // TODO: implement fields mapping
            CreateMap<CoverImageModel, CoverImage>();
        }
    }
}
