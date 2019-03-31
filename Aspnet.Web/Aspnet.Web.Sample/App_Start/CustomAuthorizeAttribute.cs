using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Aspnet.Web.Sample
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string _permission;
        private bool _onlyLogin;
        public CustomAuthorizeAttribute(string permission = null)
        {
            _permission = permission;
            _onlyLogin = string.IsNullOrEmpty(_permission);
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                ClaimsPrincipal claimsPrincipal = (ClaimsPrincipal) user;
                if (_onlyLogin || claimsPrincipal.Claims.Any(claim =>
                    claim.Type == CustomAuthorizeOption.AuthorizeType && claim.Value == _permission))
                {
                    return;
                }
            }
            filterContext.Result = new HttpUnauthorizedResult("未授权");
        }
    }

    public class CustomAuthorizeOption
    {
        public static readonly string AuthorizeType = "Permission";
    }
}
