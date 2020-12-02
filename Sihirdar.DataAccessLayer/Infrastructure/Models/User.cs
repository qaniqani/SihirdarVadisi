using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class User
    {
        public int Id { get; set; }
        public string DefaultLanguage { get; set; }
        public int DefaultLanguageId { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string Gsm { get; set; }
        public string InterestAreas { get; set; }
        public GenderTypes Gender { get; set; }
        public string Password { get; set; }
        public string PicturePath { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public DateTime BirthDate { get; set; }
        public string ActivationCode { get; set; }
        public string BannedMessage { get; set; }
        public string FacebookDetail { get; set; }
        public SignInTypes SignInType { get; set; }
        public UserStatusTypes Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}