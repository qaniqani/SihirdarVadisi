using System.Web.Mvc;
using AdminProject.Helpers;

namespace AdminProject.Attributes
{
    public class AuthorizationFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Tool.UserCheck() == null)
            {
                var url = "/";
                filterContext.Result = new RedirectResult(url);
            }

            base.OnActionExecuting(filterContext);
        }
    }
}