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
            CreateMap<NewBookModel, Book>()
                .ForMember(x => x.CoverImage, x => x.Ignore());

            CreateMap<Book, BookModel>();
            CreateMap<CoverImage, FileContent>();
        }
    }
}
