using System.Linq;
using System.Web.Mvc;
using AdminProject.Helpers;
using Sihirdar.DataAccessLayer.Infrastructure;
using Sihirdar.DataAccessLayer;

namespace AdminProject.Attributes
{
    public class CookieCheckAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Request.Cookies["User"] != null)
            {
                if (Tool.UserCheck() == null)
                {
                    var cookie = filterContext.HttpContext.Request.Cookies["User"];
                    var email = cookie.Values["Email"];
                    if (!string.IsNullOrEmpty(email))
                    {
                        var db = new AdminDbContext("AdminDbContext");
                        var user = db.Users.FirstOrDefault(a => a.Email == email);
                        if (user?.Status == UserStatusTypes.Active)
                            if (filterContext.HttpContext.Session != null)
                                filterContext.HttpContext.Session["User"] = user;
                    }
                }
            }
            base.OnActionExecuting(filterContext);
        }
    }
}