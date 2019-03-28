using Aspnet.Web.Common;
using System;
using System.Security.Claims;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Aspnet.Web.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            //AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
            AntiForgeryConfig.SuppressIdentityHeuristicChecks = true;
            filters.Add(new IdentityFilter());

            filters.Add(new HandleErrorAttribute());
        }
    }

    public class IdentityFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                var ticket = FormsAuthentication.Decrypt(cookie.Value);
                FormsIdentity formsIdentity = new FormsIdentity(ticket);
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(formsIdentity);

                claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, ticket.UserData));

                ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                filterContext.HttpContext.User = claimsPrincipal;
            }
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            ConnectionManager.ConnectionDispose();
        }
    }
}