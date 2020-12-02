using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.Service.Draw.Exception;
using Sihirdar.Service.Draw.Service.Interface;
using Sihirdar.Service.Draw.ServiceModel;
using Sihirdar.Service.Draw.Utility;

namespace Sihirdar.Service.Draw.Service
{
    public class UserService : IUserService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public UserService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public IEnumerable<UserAddResult> Add(UserAddRequest request)
        {
            var db = _dbFactory();

            if (request.AutoGenerateGuid)
                request.Users.ForEach(d => d.UserGuid = Utililty.GenerateCardNumber());

            if (!request.AutoGenerateGuid)
            {
                var checkNull = request.Users.Where(a => string.IsNullOrEmpty(a.UserGuid)).ToList();
                if (checkNull.Count > 0)
                    throw new UserGuidNotNullException(JsonConvert.SerializeObject(checkNull));
            }

            var users = request.Users.Select(a =>
            {
                var user = new DrawUser
                {
                    ApiKey = request.ApiKey,
                    DrawId = request.DrawId,
                    Email = a.Email,
                    MemberId = request.MemberId,
                    Name = a.Name,
                    UserGuid = a.UserGuid
                };

                return user;
            }).ToList();

            try
            {
                db.DrawUsers.AddRange(users);
                db.SaveChanges();

                var result = users.Select(a => new UserAddResult
                {
                    Email = a.Email,
                    Name = a.Name,
                    UserGuid = a.UserGuid
                });

                return result;
            }
            catch (System.Exception ex)
            {
                throw new EntityException(ex.Message);
            }
        }

        public IEnumerable<UserListResult> List(UserListRequest request)
        {
            var db = _dbFactory();
            var users = db.DrawUsers.Where(a => a.ApiKey == request.ApiKey && a.DrawId == request.DrawId && a.MemberId == request.MemberId).ToList();

            if (users.Count == 0)
                return Enumerable.Empty<UserListResult>();

            var result = users.Select(a => new UserListResult
            {
                Email = a.Email,
                Name = a.Name,
                UserGuid = a.UserGuid,
                Win = a.Win
            });

            return result;
        }

        public DrawUser GetWinUser(UserGetWinRequest request)
        {
            var db = _dbFactory();
            var user = db.DrawUsers.FirstOrDefault(a => a.ApiKey == request.ApiKey && a.DrawId == request.DrawId && a.Win);

            return user;
        }

        public UserWinResult Win(UserWinRequest request)
        {
            var db = _dbFactory();
            try
            {
                var user = db.DrawUsers.FirstOrDefault(a => a.ApiKey == request.ApiKey && a.UserGuid == request.UserGuid);
                if (user == null)
                    throw new UserNotFoundException();

                user.Win = true;
                db.SaveChanges();

                return new UserWinResult { Result = true };
            }
            catch (UserNotFoundException)
            {
                throw new UserNotFoundException();
            }
            catch (System.Data.EntityException ex)
            {
                throw new EntityException(ex.Message);
            }
            catch (System.Exception ex)
            {
                throw new EntityException(ex.Message);
            }
        }
    }
}