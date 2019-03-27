using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aspnet.Web.Common
{
    public static class WebExtensions
    {
        private static readonly MD5 _md5 = System.Security.Cryptography.MD5.Create();

        public static string MD5(this string value)
        {
            return Convert.ToBase64String(_md5.ComputeHash(Encoding.UTF8.GetBytes(value)));
        }
    }
}
