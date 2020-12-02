using System;
using System.Collections.Generic;
using System.Linq;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.Service.Draw.Exception;
using Sihirdar.Service.Draw.Service.Interface;
using Sihirdar.Service.Draw.ServiceModel;

namespace Sihirdar.Service.Draw.Service
{
    public class DefinitionService : IDefinitionService
    {
        private readonly Func<AdminDbContext> _dbFactory;
        private readonly IUserService _userService;

        public DefinitionService(Func<AdminDbContext> dbFactory, IUserService userService)
        {
            _dbFactory = dbFactory;
            _userService = userService;
        }

        public DefinitionAddResult Add(DefinitionAddRequest request)
        {
            var db = _dbFactory();

            var definition = new DrawDefinition
            {
                ApiKey = request.ApiKey,
                Detail = request.Detail,
                EndDate = request.EndDate,
                MemberId = request.MemberId,
                Name = request.Name,
                StartDate = request.StartDate,
                WinCount = request.WinCount
            };

            try
            {
                db.DrawDefinitions.Add(definition);
                db.SaveChanges();

                return new DefinitionAddResult { Result = true };
            }
            catch (System.Exception ex)
            {
                throw new EntityException(ex.Message);
            }
        }

        public IEnumerable<DefinitionListResult> List(DefinitionListRequest request)
        {
            if(request.Status == DataAccessLayer.StatusTypes.Freeze)
                return Enumerable.Empty<DefinitionListResult>();

            var db = _dbFactory();

            var definitions =
                db.DrawDefinitions.Where(a => a.ApiKey == request.ApiKey && a.MemberId == request.MemberId && a.StartDate >= request.StartDate && a.EndDate >= request.EndDate && a.Status == request.Status).ToList();

            if (definitions.Count == 0)
                return Enumerable.Empty<DefinitionListResult>();

            var result = definitions.Select(a => new DefinitionListResult
            {
                Id = a.Id,
                CreatedDate = a.CreatedDate,
                Detail = a.Detail,
                EndDate = a.EndDate,
                Name = a.Name,
                StartDate = a.StartDate,
                WinCount = a.WinCount
            });
            return result;
        }

        public IEnumerable<DefinitionListResult> ActiveList(DefinitionActiveListRequest request)
        {
            var db = _dbFactory();

            var definitions =
                db.DrawDefinitions.Where(a => a.ApiKey == request.ApiKey && a.MemberId == request.MemberId && a.StartDate >= request.StartDate && a.EndDate >= request.EndDate && a.Status == DataAccessLayer.StatusTypes.Active).ToList();

            if (definitions.Count == 0)
                return Enumerable.Empty<DefinitionListResult>();

            var result = definitions.Select(a => new DefinitionListResult
            {
                Id = a.Id,
                CreatedDate = a.CreatedDate,
                Detail = a.Detail,
                EndDate = a.EndDate,
                Name = a.Name,
                StartDate = a.StartDate,
                WinCount = a.WinCount
            });
            return result;
        }

        public DefinitionChangeStatusResult ChangeStatus(DefinitionChangeStatusRequest request)
        {
            var db = _dbFactory();
            var definition = db.DrawDefinitions.FirstOrDefault(a => a.Id == request.DefinitionId && a.ApiKey == request.ApiKey);
            if (definition == null)
                throw new DefinitionNotFoundException();

            try
            {
                definition.Status = request.Status;
                db.SaveChanges();

                return new DefinitionChangeStatusResult { Result = true };
            }
            catch (System.Exception exception)
            {
                throw new EntityException(exception.Message);
            }
        }

        public UserListResult GetWinUser(DefinitionGetWinUserRequest request)
        {
            var db = _dbFactory();
            var definition = db.DrawDefinitions.FirstOrDefault(a => a.ApiKey == request.ApiKey && a.Id == request.Id);
            if (definition == null)
                throw new DefinitionNotFoundException();

            var userRequest = new UserGetWinRequest
            {
                ApiKey = request.ApiKey,
                DrawId = definition.Id
            };

            var user = _userService.GetWinUser(userRequest);
            if (user == null)
                throw new UserNotFoundException();

            var result = new UserListResult
            {
                Email = user.Email,
                Name = user.Name,
                UserGuid = user.UserGuid,
                Win = user.Win
            };
            return result;
        }
    }
}