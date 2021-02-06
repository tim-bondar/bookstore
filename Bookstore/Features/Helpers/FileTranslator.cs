using System.IO;
using DB.Entities;
using Microsoft.AspNetCore.Http;

namespace Features.Helpers
{
    public static class FileTranslator
    {
        public static void CopyToBook(this IFormFile file, Book book)
        {
            book.ImageContentType = file.ContentType;
            using var ms = new MemoryStream();
            file.CopyTo(ms);
            book.ImageContent = ms.ToArray();
        }
    }
}
