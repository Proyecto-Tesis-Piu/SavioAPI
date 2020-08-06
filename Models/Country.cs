﻿using System.ComponentModel.DataAnnotations;

namespace SavioAPI.Models
{
    public class Country
    {
        [StringLength(3)]
        [Key]
        public string CountryCode { get; set; }
        public string Name { get; set; }
    }
}