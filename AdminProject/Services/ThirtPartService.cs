using System;
using System.Linq;
using AdminProject.Models;
using AdminProject.Services.Interface;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services
{
    public class ThirtPartService : IThirtPartService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public ThirtPartService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public bool AddTournament(TournamentSaveModelDto request)
        {
            var userTeam = new UserTeam
            {
                BackupUsername1 = request.BackupUsername1,
                BackupUsername2 = request.BackupUsername2,
                BackupUserNick1 = request.BackupUserNick1,
                BackupUserNick2 = request.BackupUserNick2,
                CreteDate = DateTime.Now,
                GameName = request.GameName,
                Phone = request.Phone,
                UserId = request.UserId,
                Username1 = request.Username1,
                Username2 = request.Username2,
                Username3 = request.Username3,
                Username4 = request.Username4,
                Username5 = request.Username5,
                UserNick1 = request.UserNick1,
                UserNick2 = request.UserNick2,
                UserNick3 = request.UserNick3,
                UserNick4 = request.UserNick4,
                UserNick5 = request.UserNick5,
                TeamName = request.TeamName,
                TournamentId = 2
            };

            try
            {
                var db = _dbFactory();
                db.UserTeams.Add(userTeam);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool EditTournament(TournamentSaveModelDto request, int userId)
        {
            var db = _dbFactory();
            var userTeam = db.UserTeams.FirstOrDefault(a => request.Id == a.Id && request.UserId == userId);
            if (userTeam == null)
                return false;

            userTeam.BackupUsername1 = request.BackupUsername1;
            userTeam.BackupUsername2 = request.BackupUsername2;
            userTeam.BackupUserNick1 = request.BackupUserNick1;
            userTeam.BackupUserNick2 = request.BackupUserNick2;
            userTeam.CreteDate = DateTime.Now;
            userTeam.GameName = request.GameName;
            userTeam.Phone = request.Phone;
            userTeam.UserId = request.UserId;
            userTeam.Username1 = request.Username1;
            userTeam.Username2 = request.Username2;
            userTeam.Username3 = request.Username3;
            userTeam.Username4 = request.Username4;
            userTeam.Username5 = request.Username5;
            userTeam.UserNick1 = request.UserNick1;
            userTeam.UserNick2 = request.UserNick2;
            userTeam.UserNick3 = request.UserNick3;
            userTeam.UserNick4 = request.UserNick4;
            userTeam.UserNick5 = request.UserNick5;
            userTeam.TeamName = request.TeamName;
            userTeam.TournamentId = 2;

            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public TournamentSaveModelDto GetTournament(int userId)
        {
            var db = _dbFactory();
            var tournament = db.UserTeams.FirstOrDefault(a => a.UserId == userId);

            if (tournament == null)
                return null;

            var tournamentDto = new TournamentSaveModelDto
            {
                BackupUsername1 = tournament.BackupUsername1,
                BackupUsername2 = tournament.BackupUsername2,
                BackupUserNick1 = tournament.BackupUserNick1,
                BackupUserNick2 = tournament.BackupUserNick2,
                GameName = tournament.GameName,
                Id = tournament.Id,
                Phone = tournament.Phone,
                TeamName = tournament.TeamName,
                UserId = tournament.UserId,
                Username1 = tournament.Username1,
                Username2 = tournament.Username2,
                Username3 = tournament.Username3,
                Username4 = tournament.Username4,
                Username5 = tournament.Username5,
                UserNick1 = tournament.UserNick1,
                UserNick2 = tournament.UserNick2,
                UserNick3 = tournament.UserNick3,
                UserNick4 = tournament.UserNick4,
                UserNick5 = tournament.UserNick5
            };

            return tournamentDto;
        }
    }
}