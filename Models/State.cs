using System.ComponentModel.DataAnnotations;

namespace SavioAPI.Models
{
    public class State
    {
        public string Name { get; set; }
        [Key]
        public string Code { get; set; }
        public string CountryCode { get; set; }
    }
}