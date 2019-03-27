using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Aspnet.Web.Sample
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new IdentityFilter());
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
    }
}