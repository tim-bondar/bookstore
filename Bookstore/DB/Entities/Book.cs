using System;
using System.ComponentModel.DataAnnotations;

namespace DB.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        [StringLength(128)]
        public string Author { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public byte[] ImageContent { get; set; }

        [Required]
        public string ImageContentType { get; set; }
    }
}
