using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SavioAPI.Models
{
    [Serializable]
    public class User
    {
        [JsonConstructor]
        public User(string FirstName, string LastName, Country Country, State State, int Age, bool Sex, 
            string Job, string CivilStateString, string Email, string Password) {
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Country = Country;
            this.State = State;
            this.Age = Age;
            this.Sex = Sex;
            this.Job = Job;
            this.CivilStateString = CivilStateString; 
            this.Email = Email;
            this.Password = Password;
            this.CountryCode = this.Country.CountryCode;
            this.StateCode = this.State.StateCode;
            switch (CivilStateString) {
                case "Single":
                    CivilStatebyte = Convert.ToByte(CivilState.Single);
                    break;
                default:
                    CivilStatebyte = Convert.ToByte(CivilState.Married);
                    break;
            }
        }
        
        public Guid Id { get; set; }
        [Column("First Name")]
        public string FirstName { get; set; }
        [Column("Last Name")]
        public string LastName { get; set; }
        [NotMapped]
        public Country Country { get; set; }
        [NotMapped]
        public State State { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }
        public string Job { get; set; }
        [NotMapped]
        public string CivilStateString { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        
        [Required]
        [StringLength(3)]
        [Column("CountryCode")]
        public string CountryCode { get; set; }

        [Required]
        [StringLength(3)]
        [Column("StateCode")]
        public string StateCode { get; set; }

        [Required]
        [Column("CivilState", TypeName = "tinyint")]
        public Byte CivilStatebyte { get; set; }
    }

    internal enum CivilState
    {
        Single,
        Married,
        InARelationship,
        FreeUnion,
    }
}