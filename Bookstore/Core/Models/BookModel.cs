﻿using System;

namespace Core.Models
{
    public class BookModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public decimal Price { get; set; }
        public byte[] ImageContent { get; set; }
        public string ImageContentType { get; set; }
    }
}
