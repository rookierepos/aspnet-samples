using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Aspnet.Web.Sample.Models
{
    public class LoginModel
    {
        [Required, DisplayName("用户名"), StringLength(20, MinimumLength = 6)]
        public string UserName { get; set; }
        [Required, DisplayName("密码"),  StringLength(12, MinimumLength = 6)]
        public string Password { get; set; }
    }
}