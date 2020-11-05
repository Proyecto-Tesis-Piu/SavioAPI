using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonetaAPI.Models
{
    public class Transaction
    {
        [Key]
        [Column("Transaction Id")]
        public Nullable<Guid> Id { get; set; }

        [Required]
        [Column("User Id")]
        public Guid UserId { get; set; }

        [DataType(DataType.Currency)]
        [Required]
        [Column("Amount")]
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public Decimal Amount { get; set; }

        [StringLength(2000)]
        [Column("Concept")]
        [Required]
        public String Concept { get; set; }
                
        [Required]
        [Column("Date")]
        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Column("Category Id")]
        [Required]
        public Guid Category { get; set; }
    }
}
