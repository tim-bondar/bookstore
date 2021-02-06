using System;
using Microsoft.AspNetCore.Http;

namespace Core.Models
{
    public class AddBookModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public IFormFile Image { get; set; }
    }
}
