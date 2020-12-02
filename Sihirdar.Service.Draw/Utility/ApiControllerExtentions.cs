using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Results;
using Sihirdar.Service.Draw.Models;
using ModelError = System.Web.Http.ModelBinding.ModelError;
using ModelStateDictionary = System.Web.Http.ModelBinding.ModelStateDictionary;

namespace Sihirdar.Service.Draw.Utility
{
    public static class ApiControllerExtentions
    {
        public static ValidationApiError ValidationErrror(this ModelStateDictionary modelStateDictionary)
        {
            var errors = modelStateDictionary.ToDictionary(
                ms => ms.Key.Replace("request.", ""),
                ms => ms.Value.Errors.Select(GetMessage).ToList());

            return new ValidationApiError(errors)
            {
                Message = "Validation there are errors"
            };
        }

        public static ValidationApiError ValidationErrror(this System.Web.Mvc.ModelStateDictionary modelStateDictionary)
        {
            var errors = modelStateDictionary.ToDictionary(
                ms => ms.Key.Replace("request.", ""),
                ms => ms.Value.Errors.Select(GetMessage).ToList());

            return new ValidationApiError(errors)
            {
                Message = "Validation there are errors"
            };
        }

        private static string GetMessage(System.Web.Mvc.ModelError error)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                return error.ErrorMessage;

            return error.Exception?.Message ?? "Unknown error cause";
        }

        private static string GetMessage(ModelError error)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                return error.ErrorMessage;

            return error.Exception?.Message ?? "Unknown error cause";
        }

        public static IHttpActionResult ValidationErrror(this ApiController apiController, ModelStateDictionary modelStateDictionary)
        {
            var errors = ValidationErrror(modelStateDictionary);
            return ValidationErrror(apiController, errors);
        }

        public static IHttpActionResult ValidationErrror(this ApiController apiController, ValidationApiError errors)
        {
            return ApiError(apiController, HttpStatusCode.BadRequest, (ApiError) errors);
        }

        public static IHttpActionResult ApiError(this ApiController apiController, ApiError error)
        {
            error.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content(apiController, HttpStatusCode.BadRequest, error);
        }

        public static IHttpActionResult ApiError(this ApiController apiController, HttpStatusCode httpStatus, ApiError error)
        {
            error.StatusCode = (int)httpStatus;
            return Content(apiController, httpStatus, error);
        }

        public static IHttpActionResult ForbiddenApiError(this ApiController apiController)
        {
            var statusCode = HttpStatusCode.Forbidden;
            return Content(apiController, statusCode, new ApiError
            {
                StatusCode = (int)statusCode,
                Code = "NotAuthorized",
                Message = "You do not have enough permisson for this action."
            });
        }

        public static IHttpActionResult ApiError<T>(this ApiController apiController, ApiError<T> error)
        {
            error.StatusCode = (int)HttpStatusCode.BadRequest;
            return Content(apiController, HttpStatusCode.BadRequest, error);
        }

        public static IHttpActionResult ApiError<T>(this ApiController apiController, HttpStatusCode httpStatus, ApiError<T> error)
        {
            error.StatusCode = (int)httpStatus;
            return Content(apiController, httpStatus, error);
        }

        private static IHttpActionResult Content<T>(ApiController apiController, HttpStatusCode httpStatus, T value)
        {
            return new NegotiatedContentResult<T>(httpStatus, value, apiController);
        }
    }
}