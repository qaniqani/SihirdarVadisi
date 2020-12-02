using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Models
{
    public class UserSearchRequestDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public UserStatusTypes Status { get; set; } = UserStatusTypes.Active;
        public int Skip { get; set; } = 1;
        public int Take { get; set; } = 20;
    }
}