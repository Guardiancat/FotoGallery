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
        public string Url { get; set; } = string.Empty;

        [Required]
        public string FilePath { get; set; }= string.Empty;

        [Required]
        public string UploadedBy { get; set; }=string.Empty;

        [Required]
        public DateTime UploadDate { get; set; }

        public string Description { get; set; }=string.Empty ;

        public ICollection<Comment> Comments { get; set; }=new List<Comment>();

        public DateTime UploadedAt { get; set; }
    }
}
