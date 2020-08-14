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
        //[JsonConstructor]
        //public ApplicationUser(string FirstName, string LastName, Country Country, State State, int Age, bool Sex, 
        //    string Job, string CivilStateString, string Email, string Password) {
        //    this.FirstName = FirstName;
        //    this.LastName = LastName;
        //    this.Country = Country;
        //    this.State = State;
        //    this.Age = Age;
        //    this.Sex = Sex;
        //    this.Job = Job;
        //    this.CivilStateString = CivilStateString; 
        //    this.Email = Email;
        //    this.Password = Password;
        //    this.CountryCode = this.Country.CountryCode;
        //    this.StateCode = this.State.StateCode;
        //    switch (CivilStateString) {
        //        case "Single":
        //            CivilStatebyte = Convert.ToByte(CivilState.Single);
        //            break;
        //        default:
        //            CivilStatebyte = Convert.ToByte(CivilState.Married);
        //            break;
        //    }
        //}

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
        [Column("Age", TypeName = "tinyint")]
        public Byte Age { get; set; }
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

        [StringLength(3)]
        [Column("StateCode", TypeName = "char(3)")]
        public string StateCode { get; set; }

        [Column("CivilState", TypeName = "tinyint")]
        public Byte CivilStatebyte { get; set; }

        [NotMapped]
        public String Password { get; set; }
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