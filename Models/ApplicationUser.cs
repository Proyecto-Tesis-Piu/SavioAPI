using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavioAPI.Models
{
    [Serializable]
    public class ApplicationUser : IdentityUser
    {
        [Column("First Name", TypeName = "varchar(80)")]
        public string FirstName { get; set; }
        
        [Column("Last Name", TypeName = "varchar(80)")]
        public string LastName { get; set; }

        [NotMapped]
        [ForeignKey("CountryCode")]
        public Country Country { get; set; }

        [NotMapped]
        [ForeignKey("StateCode")]
        public State State { get; set; }

        [Column("Sex", TypeName = "bit")]
        public bool SexBit { get; set; }
        
        [NotMapped]
        public string Sex { get; set; }

        [Column("Job", TypeName = "varchar(100)")]
        public string Job { get; set; }
        
        [NotMapped]
        public string CivilStateString { get; set; }

        [StringLength(3)]
        [Column("CountryCode", TypeName = "char(3)")]
        public string CountryCode { get; set; }

        [StringLength(5)]
        [Column("StateCode", TypeName = "char(5)")]
        public string StateCode { get; set; }

        [Column("CivilState", TypeName = "tinyint")]
        public Byte CivilStatebyte { get; set; }

        [NotMapped]
        public String Password { get; set; }

        [DisplayFormat(DataFormatString = "{0:O}", ApplyFormatInEditMode = true)]
        [Column("BirthDate", TypeName = "date")]
        public DateTime BirthDate { get; set; }
    }

    internal enum CivilState
    {
        Single,
        Married,
        Divorced,
        Widowed,
        FreeUnion,
    }
}