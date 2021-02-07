using System.IO;
using DB.Entities;
using Microsoft.AspNetCore.Http;

namespace Features.Helpers
{
    public static class FileTranslator
    {
        public static CoverImage ToCoverImage(this IFormFile file)
        {
            if (file.Length > 0)
            {
                var img = new CoverImage
                {
                    ContentType = file.ContentType
                };

                using var ms = new MemoryStream();
                file.CopyTo(ms);
                img.Content = ms.ToArray();
                return img;
            }

            return null;
        }
    }
}
