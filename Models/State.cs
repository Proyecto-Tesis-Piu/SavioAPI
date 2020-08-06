using System.ComponentModel.DataAnnotations;

namespace SavioAPI.Models
{
    public class State
    {
        public string Name { get; set; }

        [StringLength(3)]
        [Key]
        public string StateCode { get; set; }

        [StringLength(3)]
        public string CountryCode { get; set; }
    }
}