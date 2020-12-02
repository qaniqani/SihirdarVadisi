using System;
using System.Web.Http;
using System.Web.Http.Description;
using Sihirdar.Service.Draw.Exception;
using Sihirdar.Service.Draw.Models;
using Sihirdar.Service.Draw.Service.Interface;
using Sihirdar.Service.Draw.ServiceModel;
using Sihirdar.Service.Draw.Utility;

namespace Sihirdar.Service.Draw.Controllers
{
    [RoutePrefix("api/draw")]
    public class DrawController : ApiController
    {
        private readonly IDefinitionService _definitionService;
        private readonly IMemberService _memberService;

        public DrawController(IDefinitionService definitionService, IMemberService memberService)
        {
            _definitionService = definitionService;
            _memberService = memberService;
        }

        [HttpPost]
        [Route("create")]
        [ResponseType(typeof(DefinitionAddResult))]
        public IHttpActionResult CreateDraw(DefinitionCreateDto request)
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

            DateTime sDate;
            DateTime eDate;
            if (!Utililty.DateTimeParsing(request.StartDate, out sDate))
                return this.ApiError(new ApiError
                {
                    Code = "DateTimeFormatExcetion",
                    StatusCode = 500,
                    Message = "Start date format is error. Formats: \"dd.MM.yyyy\", \"dd.MM.yyyy HH:mm\", \"dd.MM.yyyy HH:mm:ss\""
                });

            if (!Utililty.DateTimeParsing(request.EndDate, out eDate))
                return this.ApiError(new ApiError
                {
                    Code = "DateTimeFormatExcetion",
                    StatusCode = 500,
                    Message = "End date format is error. Formats: \"dd.MM.yyyy\", \"dd.MM.yyyy HH:mm\", \"dd.MM.yyyy HH:mm:ss\""
                });

            try
            {
                var definition = new DefinitionAddRequest
                {
                    ApiKey = request.ApiKey,
                    Detail = request.Detail,
                    EndDate = eDate,
                    MemberId = memberId,
                    Name = request.Name,
                    StartDate = sDate,
                    WinCount = request.WinCount
                };

                var result = _definitionService.Add(definition);

                return Ok(result);
            }
            catch (EntityException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "EntityException",
                    StatusCode = 500,
                    Message = $"There was an error creating your draw. Error Detail: {ex.Message}."
                });
            }
        }

        [HttpPost]
        [Route("list")]
        [ResponseType(typeof(DefinitionListResult))]
        public IHttpActionResult DrawList(DrawListRequestDto request)
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

            var list = new DefinitionListRequest
            {
                ApiKey = request.ApiKey,
                MemberId = memberId,
                Status = request.Status
            };

            if (!string.IsNullOrEmpty(request.StartDate) && !string.IsNullOrEmpty(request.EndDate))
            {
                DateTime sDate;
                DateTime eDate;

                if (!Utililty.DateTimeParsing(request.StartDate, out sDate))
                    return this.ApiError(new ApiError
                    {
                        Code = "DateTimeFormatExcetion",
                        StatusCode = 500,
                        Message = "Start date format is error. Formats: \"dd.MM.yyyy\", \"dd.MM.yyyy HH:mm\", \"dd.MM.yyyy HH:mm:ss\""
                    });

                if (!Utililty.DateTimeParsing(request.EndDate, out eDate))
                    return this.ApiError(new ApiError
                    {
                        Code = "DateTimeFormatExcetion",
                        StatusCode = 500,
                        Message = "End date format is error. Formats: \"dd.MM.yyyy\", \"dd.MM.yyyy HH:mm\", \"dd.MM.yyyy HH:mm:ss\""
                    });

                list.StartDate = sDate;
                list.EndDate = eDate;
            }

            try
            {
                var result = _definitionService.List(list);

                return Ok(result);
            }
            catch (EntityException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "EntityException",
                    StatusCode = 500,
                    Message = $"There was an error getting your draws. Error Detail: {ex.Message}."
                });
            }
        }

        [HttpPost]
        [Route("status/change")]
        [ResponseType(typeof(DefinitionChangeStatusResult))]
        public IHttpActionResult DrawStatusChange(DrawStatusChangeRequestDto request)
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
                var status = new DefinitionChangeStatusRequest
                {
                    ApiKey = request.ApiKey,
                    DefinitionId = request.DefinitionId,
                    Status = request.Status
                };

                var result = _definitionService.ChangeStatus(status);

                return Ok(result);
            }
            catch (DefinitionNotFoundException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "DefinitionNotFoundException",
                    StatusCode = 500,
                    Message = "Draw is not found. Invalid id: " + request.DefinitionId
                });
            }
            catch (EntityException ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "EntityException",
                    StatusCode = 500,
                    Message = $"There was an error change your draw status. Error Detail: {ex.Message}."
                });
            }
        }

        [HttpPost]
        [Route("win")]
        [ResponseType(typeof(UserListResult))]
        public IHttpActionResult GetDrawWin(DrawWinUserRequestDto request)
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
                var win = new DefinitionGetWinUserRequest
                {
                    ApiKey = request.ApiKey,
                    Id = request.Id
                };

                var result = _definitionService.GetWinUser(win);

                return Ok(result);
            }
            catch (DefinitionNotFoundException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "DefinitionNotFoundException",
                    StatusCode = 500,
                    Message = "Draw not found. Draw id: " + request.Id
                });
            }
            catch (UserNotFoundException)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UserNotFoundException",
                    StatusCode = 500,
                    Message = "Winner of the contest was not found."
                });
            }
            catch (System.Exception ex)
            {
                return this.ApiError(new ApiError
                {
                    Code = "UnhandledException",
                    StatusCode = 500,
                    Message = $"Unhandled exception. Exception Detail: {ex.Message}."
                });
            }
        }
    }
}
