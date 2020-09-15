using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SavioAPI.Models
{
    public class CategoryDto
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [StringLength(2000)]
        [Column("Name")]
        [Required]
        public String Concept { get; set; }

        [Column("Is Expense", TypeName = "bit")]
        [Required]
        public bool IsExpense { get; set; }

        [StringLength(50)]
        [Column("icon")]
        [Required]
        public String Icon { get; set; }

        [Column("User Id")]
        public Guid? UserId { get; set; }

        [NotMapped]
        public Transaction[] ChildrenTransactions { get; set; }

        [Column("Percentage")]
        public Decimal Percentage { get; set; }

        [Column("Total")]
        public Decimal Amount { get; set; }
    }
}
