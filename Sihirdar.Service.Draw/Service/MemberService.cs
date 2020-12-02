using System;
using System.Linq;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer.Infrastructure.Models;
using Sihirdar.Service.Draw.Exception;
using Sihirdar.Service.Draw.ServiceModel;
using Sihirdar.Service.Draw.Utility;
using Sihirdar.Service.Draw.Service.Interface;

namespace Sihirdar.Service.Draw.Service
{
    public class MemberService : IMemberService
    {
        private readonly Func<AdminDbContext> _dbFactory;

        public MemberService(Func<AdminDbContext> dbFactory)
        {
            _dbFactory = dbFactory;
        }

        public MemberResult Add(MemberRequest request)
        {
            var db = _dbFactory();

            var apiKey = Utililty.ApiKeyGenerator();

            var member = new DrawMember
            {
                ApiKey = apiKey,
                Email = request.Email,
                Gsm = request.Gsm,
                Name = request.Name,
                Password = request.Password,
                ProjectDetail = request.ProjectDetail,
                SiteUrl = request.SiteUrl,
                Surname = request.Surname,
                Title = request.Title,
                Username = request.Username
            };

            try
            {
                db.DrawMembers.Add(member);
                db.SaveChanges();

                return new MemberResult
                {
                    ApiKey = apiKey
                };
            }
            catch (System.Exception ex)
            {
                throw new EntityException(ex.Message);
            }
        }

        public MemberResult Login(MemberLoginRequest request)
        {
            var db = _dbFactory();

            var member = db.DrawMembers.FirstOrDefault(a => a.Username == request.Username && a.Password == request.Password);
            if (member == null)
                throw new MemberNotFoundException();

            if (member.StatusType != DataAccessLayer.DrawApiStatusTypes.Active)
            {
                switch (member.StatusType)
                {
                    case DataAccessLayer.DrawApiStatusTypes.Banned:
                        throw new MemberBannedException();
                    case DataAccessLayer.DrawApiStatusTypes.Deactive:
                        throw new MemberDeactiveException();
                    case DataAccessLayer.DrawApiStatusTypes.Deleted:
                        throw new MemberDeletedException();
                    case DataAccessLayer.DrawApiStatusTypes.OldApi:
                        throw new MemberOldApiException();
                    case DataAccessLayer.DrawApiStatusTypes.Pending:
                        throw new MemberPendingException();
                    case DataAccessLayer.DrawApiStatusTypes.Unapproved:
                        throw new MemberUnapprovedException();
                }
            }

            return new MemberResult
            {
                ApiKey = member.ApiKey
            };
        }

        public bool Check(MemberCheckApiKeyRequest request, out int id)
        {
            var db = _dbFactory();

            var member = db.DrawMembers.FirstOrDefault(a => a.ApiKey == request.ApiKey && a.StatusType == DataAccessLayer.DrawApiStatusTypes.Active);
            id = member?.Id ?? 0;
            return member != null;
        }
    }
}