using System;
using System.ComponentModel.DataAnnotations;

namespace DB.Entities
{
    public class CoverImage
    {
        public Guid Id { get; set; }

        [Required]
        public byte[] Content { get; set; }

        [Required]
        public string ContentType { get; set; }
    }
}
