using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MBA.Marketplace.Web.Filters
{
    public class AutorizadoAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var hasToken = context.HttpContext.Request.Cookies.ContainsKey("AccessToken");

            if (!hasToken)
            {
                context.Result = new RedirectToActionResult("Index", "Login", null);
            }
        }
    }
}
