using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Aspnet.Web.Sample.Models
{
    public class RegistryModel
    {
        [Required, DisplayName("用户名"), StringLength(20, MinimumLength = 6)]
        public string UserName { get; set; }
        [Required, DisplayName("昵称"), StringLength(20, MinimumLength = 3)]
        public string NickName { get; set; }
        [Required, DisplayName("密码"), StringLength(12, MinimumLength = 6)]
        public string Password { get; set; }
        [Required, DisplayName("再次输入"), StringLength(12, MinimumLength = 6), Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}