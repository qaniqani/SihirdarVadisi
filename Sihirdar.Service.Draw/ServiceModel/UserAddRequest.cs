using System.Collections.Generic;

namespace Sihirdar.Service.Draw.ServiceModel
{
    public class UserAddRequest
    {
        public string ApiKey { get; set; }
        public int DrawId { get; set; }
        public int MemberId { get; set; }
        public bool AutoGenerateGuid { get; set; }
        public List<UserViewModel> Users { get; set; }
    }

    public class UserViewModel
    {
        public string UserGuid { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}