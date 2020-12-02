using System;

namespace Sihirdar.DataAccessLayer.Infrastructure.Models
{
    public class UserTeam
    {
        public int Id { get; set; }
        public int TournamentId { get; set; } = 1;
        public int UserId { get; set; }
        public string GameName { get; set; }
        public string Username1 { get; set; }
        public string Username2 { get; set; }
        public string Username3 { get; set; }
        public string Username4 { get; set; }
        public string Username5 { get; set; }
        public string UserNick1 { get; set; }
        public string UserNick2 { get; set; }
        public string UserNick3 { get; set; }
        public string UserNick4 { get; set; }
        public string UserNick5 { get; set; }
        public string TeamName { get; set; }
        public string BackupUsername1 { get; set; }
        public string BackupUsername2 { get; set; }
        public string BackupUserNick1 { get; set; }
        public string BackupUserNick2 { get; set; }
        public string Phone { get; set; }
        public DateTime CreteDate { get; set; }
    }
}
