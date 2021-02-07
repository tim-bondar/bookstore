using System.IO;
using AutoMapper;
using Features;
using Microsoft.AspNetCore.Http;

namespace Tests
{
    public static class Helpers
    {
        public static IMapper CreateMapper()
        {
            var configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MapProfile>(); });

            return new Mapper(configuration);
        }

        public static IFormFile CreateTestFormFile(Stream stream, string contentType)
        {
            return new FormFile(stream, 0, stream.Length, null, "default.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
    }
}
