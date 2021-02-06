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
            CreateMap<AddBookModel, Book>()
                .ForMember(x => x.ImageContent, x => x.Ignore())
                .ForMember(x => x.ImageContentType, x => x.MapFrom(f => f.Image.ContentType));

            CreateMap<Book, BookModel>();
            CreateMap<BookModel, Book>();
        }
    }
}
