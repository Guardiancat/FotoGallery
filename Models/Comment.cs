using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoGallery.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Text { get; set; }

        [Required]
        public string? Author { get; set; }

        [Required]
        public DateTime DatePosted { get; set; }

        [Required]
        public int PhotoId { get; set; }

        public Photo? Photo { get; set; }
    }
}
