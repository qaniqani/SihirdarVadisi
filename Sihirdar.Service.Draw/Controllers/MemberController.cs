using System.Web.Http;
using System.Web.Http.Description;
using Sihirdar.Service.Draw.Exception;
using Sihirdar.Service.Draw.Models;
using Sihirdar.Service.Draw.Service.Interface;
using Sihirdar.Service.Draw.ServiceModel;
using Sihirdar.Service.Draw.Utility;

namespace Sihirdar.Service.Draw.Controllers
{
    [RoutePrefix("api/member")]
    public class MemberController : ApiController
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpPost]
        [Route("login")]
        [ResponseType(typeof(LoginResultDto))]
        public IHttpActionResult Login(LoginRequestDto request)
        {
            if (!ModelState.IsValid)
                return this.ValidationErrror(ModelState);

            var login = new MemberLoginRequest
            {
                Password = request.Password,
                Username = request.Username
            };
            try
            {
                var user = _memberService.Login(login);

                return Ok(new LoginResultDto{ ApiKey = user.ApiKey });
            }
            catch (MemberNotFoundException)
            {
                return Unauthorized();
            }
            catch (MemberBannedException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "MemberBannedException",
                    Message = "Your account is banned."
                });
            }
            catch (MemberDeactiveException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "MemberDeactiveException",
                    Message = "Your account is deactive."
                });
            }
            catch (MemberDeletedException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "MemberDeletedException",
                    Message = "Your account is deleted."
                });
            }
            catch (MemberOldApiException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "MemberOldApiException",
                    Message = "Your account is using the old key."
                });
            }
            catch (MemberPendingException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "MemberPendingException",
                    Message = "Your account is pending."
                });
            }
            catch (MemberUnapprovedException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "MemberUnapprovedException",
                    Message = "Your account is unapproved."
                });
            }
        }

        [HttpPost]
        [Route("create")]
        [ResponseType(typeof(MemberCreateResultDto))]
        public IHttpActionResult Create(MemberCreateRequestDto request)
        {
            if (!ModelState.IsValid)
                return this.ValidationErrror(ModelState);

            try
            {
                var memberRequest = new MemberRequest
                {
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

                var user = _memberService.Add(memberRequest);
                var result = new MemberCreateResultDto
                {
                    ApiKey = user.ApiKey,
                    Status = DataAccessLayer.DrawApiStatusTypes.Pending
                };

                return Ok(result);
            }
            catch (EntityException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "EntityException",
                    StatusCode = 500,
                    Message = $"There was an error creating your account. Error Detail: {ex.Message}."
                });
            }
        }
    }
}
