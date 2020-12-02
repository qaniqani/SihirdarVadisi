using Sihirdar.DataAccessLayer;

namespace AdminProject.Models
{
    public class UserCreateRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int Day { get; set; } = 1;
        public int Month { get; set; } = 1;
        public int Year { get; set; } = 1970;
        public string Picture { get; set; }
        public string InterestAreas { get; set; }
        public string Password { get; set; }
        public string Password2 { get; set; }
        public GenderTypes Gender { get; set; }
    }
}