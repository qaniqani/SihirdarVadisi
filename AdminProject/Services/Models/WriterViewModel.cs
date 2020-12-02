using AdminProject.Models;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Services.Models
{
    public class WriterViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public AdminTypes AdminType { get; set; } = AdminTypes.Admin;
        public string Picture { get; set; }
        public string Motto { get; set; }
        public string Facebook { get; set; }
        public string Twitter { get; set; }
    }
}