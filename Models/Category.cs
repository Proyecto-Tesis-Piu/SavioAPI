using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SavioAPI.Models
{
    public class Category
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [StringLength(2000)]
        [Column("Name")]
        [Required]
        public String Name { get; set; }

        [Column("Is Expense", TypeName = "bit")]
        [Required]
        public bool IsExpense { get; set; }

        [StringLength(50)]
        [Column("icon")]
        [Required]
        public Byte icon { get; set; }

        [Column("User Id")]
        [Required]
        public Guid UserId { get; set; }

        [NotMapped]
        public Transaction[] ChildrenTransactions { get; set; }
    }
}
