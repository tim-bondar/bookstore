using System;
using System.ComponentModel.DataAnnotations;
using Core;

namespace DB.Entities
{
    public class Book
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(Constants.MaxTitleSize)]
        public string Title { get; set; }

        [StringLength(Constants.MaxDescriptionSize)]
        public string Description { get; set; }

        [StringLength(Constants.MaxAuthorSize)]
        public string Author { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public byte[] ImageContent { get; set; }

        [Required]
        public string ImageContentType { get; set; }
    }
}
