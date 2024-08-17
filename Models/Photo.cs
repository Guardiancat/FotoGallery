using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace PhotoGallery.Models
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; }

        [Required]
        public string FilePath { get; set; }

        [Required]
        public string UploadedBy { get; set; }

        [Required]
        public DateTime UploadDate { get; set; }

        public string Description { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public DateTime UploadedAt { get; set; }
    }
}
