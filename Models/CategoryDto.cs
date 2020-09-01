using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SavioAPI.Models
{
    public class CategoryDto : Category
    {
        [Column("Percentage")]
        public double Percentage { get; set; }

        [Column("Total")]
        public Decimal Total { get; set; }
    }
}
