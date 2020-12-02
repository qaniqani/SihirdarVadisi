using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using AdminProject.Models;

namespace AdminProject.Controllers
{
    public class BaseController : Controller
    {
        public void SetTitle(string title)
        {
            ViewBag.Title = title;
        }

        public void RightBlockAdvert(List<CategoryAdvertDto> adverts)
        {
            ViewBag.RightBlockAdvert = adverts;
        }

        public void CenterAdvert(CategoryAdvertDto advert)
        {
            ViewBag.CenterAdvert = advert;
        }

        public void TypeAdvert(CategoryAdvertDto advert)
        {
            ViewBag.TypeAdvert = advert;
        }

        public void HeaderAdvert(CategoryAdvertDto advert)
        {
            ViewBag.HeaderAdvert = advert;
        }

        public void Home300Advert(CategoryAdvertDto advert)
        {
            ViewBag.Home300Advert = advert;
        }

        public void Home728Advert(CategoryAdvertDto advert)
        {
            ViewBag.Home728Advert = advert;
        }

        public void SetErrorMessage(string errorMessage)
        {
            TempData["ErrorDetail"] = errorMessage;
        }

        public string GetErrorMessage(ICollection<ModelState> state)
        {
            var errors = string.Join(", ", state.SelectMany(a => a.Errors.Select(e => e.ErrorMessage + "<br>")));
            return errors;
        }

        public string GetUrlAddress(string urlFormat)
        {
            var queryStringsList = Request.QueryString.ToEnumerable().Where(a => a.Key != "page").ToList();
            var nameValue = new NameValueCollection();
            queryStringsList.ForEach(a =>
            {
                nameValue.Add(a.Key, a.Value);
            });

            var nameValueQuery = nameValue.ToQueryString();

            ViewBag.UrlAddress = nameValueQuery.Any() ? $"/{urlFormat}?{nameValueQuery}" : $"/{urlFormat}";
            return ViewBag.UrlAddress;
        }
    }
}