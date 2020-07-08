using System;

namespace SavioAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Country Country { get; set; }
        public State State { get; set; }
        public int Age { get; set; }
        public bool Sex { get; set; }
        public string Job { get; set; }
        public string CivilState { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}