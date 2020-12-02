using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Sihirdar.Service.Draw.Exception;
using Sihirdar.Service.Draw.Models;
using Sihirdar.Service.Draw.Service.Interface;
using Sihirdar.Service.Draw.ServiceModel;
using Sihirdar.Service.Draw.Utility;

namespace Sihirdar.Service.Draw.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
        private readonly IMemberService _memberService;

        public UserController(IUserService userService, IMemberService memberService)
        {
            _userService = userService;
            _memberService = memberService;
        }

        [HttpPost]
        [Route("add")]
        [ResponseType(typeof(IEnumerable<UserAddResult>))]
        public IHttpActionResult Add(AddUserRequestDto request)
        {
            if (!ModelState.IsValid)
                return this.ValidationErrror(ModelState);

            int memberId;
            if (!_memberService.Check(new MemberCheckApiKeyRequest { ApiKey = request.ApiKey }, out memberId))
                return this.ApiError(new ApiError
                {
                    Code = "ApiKeyException",
                    StatusCode = 500,
                    Message = "Invalid ApiKey."
                });

            try
            {
                var users = new UserAddRequest
                {
                    ApiKey = request.ApiKey,
                    AutoGenerateGuid = request.AutoGenerateGuid,
                    DrawId = request.DrawId,
                    MemberId = memberId,
                    Users = request.Users.Select(a => new UserViewModel
                    {
                        Email = a.Email,
                        Name = a.Name,
                        UserGuid = a.UserGuid
                    }).ToList()
                };

                var result = _userService.Add(users);

                return Ok(result);
            }
            catch (UserGuidNotNullException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UserGuidNotNullException",
                    StatusCode = 500,
                    Message = $"User guid auto generator error users. User Detail: {ex.Message}."
                });
            }
            catch (EntityException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "EntityException",
                    StatusCode = 500,
                    Message = $"There was an error adding your users. Error Detail: {ex.Message}."
                });
            }
        }

        [HttpPost]
        [Route("list")]
        [ResponseType(typeof(IEnumerable<UserListResult>))]
        public IHttpActionResult List(UserListRequestDto request)
        {
            if (!ModelState.IsValid)
                return this.ValidationErrror(ModelState);

            int memberId;
            if (!_memberService.Check(new MemberCheckApiKeyRequest { ApiKey = request.ApiKey }, out memberId))
                return this.ApiError(new ApiError
                {
                    Code = "ApiKeyException",
                    StatusCode = 500,
                    Message = "Invalid ApiKey."
                });

            try
            {
                var users = new UserListRequest
                {
                    ApiKey = request.ApiKey,
                    DrawId = request.DrawId,
                    MemberId = memberId
                };

                var result = _userService.List(users);

                return Ok(result);
            }
            catch (System.Exception ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UnhandledException",
                    StatusCode = 500,
                    Message = $"Unhandled exception. Error Detail: {ex.Message}."
                });
            }
        }

        [HttpPost]
        [Route("win")]
        [ResponseType(typeof(UserWinResult))]
        public IHttpActionResult Win(UserWinRequestDto request)
        {
            if (!ModelState.IsValid)
                return this.ValidationErrror(ModelState);

            int memberId;
            if (!_memberService.Check(new MemberCheckApiKeyRequest { ApiKey = request.ApiKey }, out memberId))
                return this.ApiError(new ApiError
                {
                    Code = "ApiKeyException",
                    StatusCode = 500,
                    Message = "Invalid ApiKey."
                });

            try
            {
                var win = new UserWinRequest
                {
                    ApiKey = request.ApiKey,
                    UserGuid = request.UserGuid
                };
                var result = _userService.Win(win);

                return Ok(result);
            }
            catch (UserNotFoundException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UserNotFoundException",
                    StatusCode = 500,
                    Message = $"User not found exception."
                });
            }
            catch (System.Data.EntityException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UnhandledException",
                    StatusCode = 500,
                    Message = $"Unhandled exception. Error Detail: {ex.Message}. System.Data.EntityException"
                });
            }
            catch (System.Exception ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UnhandledException",
                    StatusCode = 500,
                    Message = $"Unhandled exception. Error Detail: {ex.Message}. System.Exception"
                });
            }
        }
    }
}
