namespace Sihirdar.Service.Draw.ServiceModel
{
    public class UserListRequest
    {
        public string ApiKey { get; set; }
        public int MemberId { get; set; }
        public int DrawId { get; set; }
    }
}