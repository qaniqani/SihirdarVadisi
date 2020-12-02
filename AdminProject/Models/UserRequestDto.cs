using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class UserRequestDto
    {
        public int Id { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gsm { get; set; }
        public string InterestAreas { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Picture { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public GenderTypes Gender { get; set; }
        public SignInTypes SignInType { get; set; }
    }
}