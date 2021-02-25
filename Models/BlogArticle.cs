using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonetaAPI.Models
{
    public class BlogArticle
    {
        [Key]
        [Column("Id")]
        [Required]
        public Guid Id { get; set; }

        [Column("Title")]
        [Required]
        public string Title { get; set; }

        [Column("ShortText")]
        public string ShortText { get; set; }
        
        [Column("Text")]
        [Required]
        public string Text { get; set; }

        [Column("Image")]
        [Required]
        public string Image { get; set; }

        [Column("Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        public DateTime DateValue { get; set; }

        [Column("Author")]
        [Required]
        public string Author { get; set; }

        [Column("Tags")]
        public string TagsString { get; set; }

        [Column("Bibliography")]
        public string BibliographyString { get; set; }

        [NotMapped]
        public string[] Tags { get; set; }

        [NotMapped]
        public string Date { get; set; }

        [NotMapped]
        public string[] Bibliography { get; set; }
    }
}
