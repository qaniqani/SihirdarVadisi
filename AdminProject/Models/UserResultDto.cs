using Sihirdar.DataAccessLayer;
using System;

namespace AdminProject.Models
{
    public class UserResultDto
    {
        public int Id { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Picture { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string Day { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string InterestAreas { get; set; }
        public GenderTypes Gender { get; set; }
        public SignInTypes SignInType { get; set; }
        public int LikeCount { get; set; } = 0;
    }
}