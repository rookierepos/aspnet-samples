using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace Aspnet.Web.Common
{
    public static class WebExtensions
    {
        private static readonly MD5 _md5 = System.Security.Cryptography.MD5.Create();

        public static string MD5(this string value)
        {
            return Convert.ToBase64String(_md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static void SignIn(this HttpContextBase context, string name, DateTime expiration, bool isPersistent,
            string userData, string cookiePath = "/")
        {
            context?.Response.Cookies.Add(new HttpCookie(
                    FormsAuthentication.FormsCookieName,
                    FormsAuthentication.Encrypt(
                        new FormsAuthenticationTicket(
                            1,
                            name,
                            DateTime.Now,
                            expiration,
                            true,
                            userData,
                            cookiePath)
                    )
                )
            );
        }

        public static void SignOut(this HttpContextBase context)
        {
            var cookie = context?.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                context.Response.Cookies.Add(cookie);
            }
        }
    }
}
