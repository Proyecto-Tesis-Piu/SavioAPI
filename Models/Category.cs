using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MonetaAPI.Models
{
    public class Category
    {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }

        [Column("Name")]
        public String Concept { get; set; }

        [Column("Is Expense", TypeName = "bit")]
        public bool IsExpense { get; set; }

        [Column("icon")]
        public String Icon { get; set; }

        [Column("User Id")]
        public Nullable<Guid> UserId { get; set; }

        [NotMapped]
        public Decimal Amount { get; set; }
    }
}
