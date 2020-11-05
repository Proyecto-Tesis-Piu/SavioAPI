using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MonetaAPI.Models
{
    public class CalendarDates
    {
        [Key]
        [Column("Date")]
        [Required]
        public DateTime Date { get; set; }

        [Column("Has Expense", TypeName = "bit")]
        [Required]
        public bool HasExpense { get; set; }

        [Column("Has Income", TypeName = "bit")]
        [Required]
        public bool HasIncome { get; set; }
    }

    public class GeneralData
    {
        [NotMapped]
        public List<CategoryDto> Transactions { get; set; }

        [Column("Income Percentage")]
        public Nullable<Decimal> IncomePercentage { get; set; }

        [DataType(DataType.Currency)]
        [Column("Income Total")]
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public Nullable<Decimal> IncomeTotal { get; set; }

        [Column("Expense Percentage")]
        public Nullable<Decimal> ExpensePercentage { get; set; }

        [DataType(DataType.Currency)]
        [Column("Expense Total")]
        [DisplayFormat(DataFormatString = "{0:#.##}", ApplyFormatInEditMode = true)]
        public Nullable<Decimal> ExpenseTotal { get; set; }
    }
}
