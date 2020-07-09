using System.ComponentModel.DataAnnotations;

namespace SavioAPI.Models
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
}