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
        public String Title { get; set; }

        [Column("ShortText")]
        public String ShortText { get; set; }
        
        [Column("Text")]
        [Required]
        public String Text { get; set; }

        [Column("Image")]
        [Required]
        public String Image { get; set; }

        [Column("Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        public DateTime DateValue { get; set; }

        [Column("Author")]
        [Required]
        public String Author { get; set; }

        [Column("Tags")]
        public String TagsString { get; set; }

        [Column("Bibliography")]
        public String BibliographyString { get; set; }

        [NotMapped]
        public String[] Tags { get; set; }

        [NotMapped]
        public String Date { get; set; }

        [NotMapped]
        public String[] Bibliography { get; set; }
    }
}
