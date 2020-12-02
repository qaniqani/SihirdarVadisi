using System.Web.Mvc;
using AdminProject.Helpers;

namespace AdminProject.Attributes
{
    public class AdminAuthAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Utility.SessionCheck() == null)
            {
                var url = "/";
                filterContext.Result = new RedirectResult(url);
            }
            else
            {
                var user = Utility.SessionCheck();
                if (!user.Authorization.Contains("Setting"))
                {
                    var url = "/";
                    filterContext.Result = new RedirectResult(url);
                }
            }



            base.OnActionExecuting(filterContext);
        }
    }
}