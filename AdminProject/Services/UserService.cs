using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AdminProject.Models;
using AdminProject.Services.CustomExceptions;
using AdminProject.Services.Interface;
using AdminProject.Services.Models;
using ClosedXML.Excel;
using Sihirdar.DataAccessLayer;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;

namespace AdminProject.Services
{
    public class UserService : IUserService
    {
        private readonly IEmailService _emailService;
        private readonly Func<AdminDbContext> _dbFactory;

        public UserService(Func<AdminDbContext> dbFactory, IEmailService emailService)
        {
            _dbFactory = dbFactory;
            _emailService = emailService;
        }

        public void Add(User user)
        {
            var db = _dbFactory();

            db.Users.Add(user);
            db.SaveChanges();
            try
            {
                _emailService.SendActivationMail(user.Email, user.Name, user.Surname, user.ActivationCode);
            }
            catch (Exception)
            {
                db.Users.Remove(user);
                db.SaveChanges();
                throw new CustomException("E-Mail gönderiminde bir hata oluştu.");
            }
        }

        public bool EmailCheck(string email)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email);
            return user == null;
        }

        public bool EmailActivation(string activationCode)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.ActivationCode == activationCode && a.Status == UserStatusTypes.Unapproved);
            if (user == null)
                return false;

            user.Status = UserStatusTypes.Active;
            db.SaveChanges();

            return true;
        }

        public void SendForgotPassword(string email)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email);
            if (user == null)
                return;

            _emailService.SendForgotPasswordMail(user.Email, user.Name, user.Surname, user.Password);
        }

        public void Edit(int id, User userRequest)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.ActivationCode = userRequest.ActivationCode;
            user.Email = userRequest.Email;
            user.Gender = userRequest.Gender;
            user.Gsm = userRequest.Gsm;
            user.Name = userRequest.Name;
            user.Password = userRequest.Password;
            user.Status = userRequest.Status;
            user.Surname = userRequest.Surname;
            db.SaveChanges();
        }

        public UserStatusTypes ChangeStatus(int id, UserStatusTypes status)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.Status = status;
            db.SaveChanges();

            return status;
        }

        public void ChangePassword(int id, string password)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            user.Password = password;
            db.SaveChanges();
        }

        public void ResetPassword(string code, string email, string password)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.ActivationCode == code && a.Email == email);
            if (user == null)
                throw new CustomException("Kullanıcı bulunamadı.");

            user.Password = password;
            db.SaveChanges();
        }

        public User Login(string email, string password)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email && a.Password == password);

            if (user == null)
                return null;

            user.LastLoginDate = DateTime.Now;
            db.SaveChanges();

            return user;
        }

        public UserActivationTypes UserActive(string activationCode)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.ActivationCode == activationCode);
            if (user == null)
                return UserActivationTypes.ActivationCodeNotFound;

            user.Status = UserStatusTypes.Active;
            user.UpdatedDate = DateTime.Now;
            db.SaveChanges();

            return UserActivationTypes.ActivationSuccess;
        }

        public User GetItem(int id)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            return user;
        }

        public User GetItem(string email)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Email == email);
            return user;
        }

        public void Delete(int id)
        {
            var db = _dbFactory();
            var user = db.Users.FirstOrDefault(a => a.Id == id);
            db.Users.Remove(user);
            db.SaveChanges();
        }

        public IList<User> List()
        {
            var db = _dbFactory();
            var users = db.Users.OrderBy(a => a.Name).ToList();
            return users;
        }

        public PagerList<User> AllUserList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var users = db.Users.OrderBy(a => a.Name).Skip(skip).Take(take).ToList();
            var userCount = db.Users.Count();

            var result = new PagerList<User>
            {
                TotalCount = userCount,
                List = users
            };

            return result;
        }

        public PagerList<User> AllUserList(UserSearchRequestDto request)
        {
            var db = _dbFactory();
            request.Skip = (request.Skip - 1) * request.Take;

            var users = db.Users.OrderBy(a => a.Name).Where(a => a.Status == request.Status);
            
            if (!string.IsNullOrEmpty(request.Email))
                users = users.Where(a => a.Email == request.Email);

            if (!string.IsNullOrEmpty(request.Name))
                users = users.Where(a => a.Name == request.Name);

            if (!string.IsNullOrEmpty(request.Surname))
                users = users.Where(a => a.Surname == request.Surname);

            var userCount = users.Count();
            var userResult = users.Skip(request.Skip).Take(request.Take).ToList();

            var result = new PagerList<User>
            {
                TotalCount = userCount,
                List = userResult
            };

            return result;
        }

        public PagerList<User> ActiveUserList(int skip, int take)
        {
            var db = _dbFactory();
            skip = (skip - 1) * take;
            var users = db.Users.Where(a => a.Status == UserStatusTypes.Active).OrderBy(a => a.Name).Skip(skip).Take(take).ToList();
            var userCount = db.Users.Count();

            var result = new PagerList<User>
            {
                TotalCount = userCount,
                List = users
            };

            return result;
        }

        public void TournamentExport(string path)
        {
            var db = _dbFactory();

            var tournaments = db.UserTeams.Select(a => new TournamentExcelDto
            {
                BackupUsername1 = a.BackupUsername1,
                BackupUsername2 = a.BackupUsername2,
                BackupUserNick1 = a.BackupUserNick1,
                BackupUserNick2 = a.BackupUserNick2,
                CreteDate = a.CreteDate,
                GameName = a.GameName,
                Phone = a.Phone,
                TeamName = a.TeamName,
                Username1 = a.Username1,
                Username2 = a.Username2,
                Username3 = a.Username3,
                Username4 = a.Username4,
                Username5 = a.Username5,
                UserNick1 = a.UserNick1,
                UserNick2 = a.UserNick2,
                UserNick3 = a.UserNick3,
                UserNick4 = a.UserNick4,
                UserNick5 = a.UserNick5
            }).ToList();

            var wb = new XLWorkbook();
            var dt = new DataTable();
            dt.Columns.Add("Oyun Adı");
            dt.Columns.Add("Takım Adı");
            dt.Columns.Add("Telefon");
            dt.Columns.Add("Kaptanın Adı");
            dt.Columns.Add("Kaptanın Nicki");
            dt.Columns.Add("1. Oyuncu Adı");
            dt.Columns.Add("2. Oyuncu Adı");
            dt.Columns.Add("3. Oyuncu Adı");
            dt.Columns.Add("4. Oyuncu Adı");
            dt.Columns.Add("1. Oyuncu Nicki");
            dt.Columns.Add("2. Oyuncu Nicki");
            dt.Columns.Add("3. Oyuncu Nicki");
            dt.Columns.Add("4. Oyuncu Nicki");
            dt.Columns.Add("1. Yedek Adı");
            dt.Columns.Add("2. Yedek Adı");
            dt.Columns.Add("1. Yedek Nicki");
            dt.Columns.Add("2. Yedek Nicki");
            dt.Columns.Add("Oluş. Tar.");

            tournaments.ForEach(a =>
            {
                var dr = dt.NewRow();
                dr[0] = a.GameName;
                dr[1] = a.TeamName;
                dr[2] = a.Phone;
                dr[3] = a.Username1;
                dr[4] = a.UserNick1;
                dr[5] = a.Username2;
                dr[6] = a.Username3;
                dr[7] = a.Username4;
                dr[8] = a.Username5;
                dr[9] = a.UserNick1;
                dr[10] = a.UserNick2;
                dr[11] = a.UserNick3;
                dr[12] = a.UserNick4;
                dr[13] = a.BackupUsername1;
                dr[14] = a.BackupUsername2;
                dr[15] = a.BackupUserNick1;
                dr[16] = a.BackupUserNick2;
                dr[17] = a.CreteDate.ToString("dd.MM.yyyy HH:mm");
                dt.Rows.Add(dr);
            });

            wb.Worksheets.Add(dt, "Turnuva");
            wb.SaveAs(path);
        }
    }
}