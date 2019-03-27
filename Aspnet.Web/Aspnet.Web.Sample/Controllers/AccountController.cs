using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Aspnet.Web.Sample.Models;
using Aspnet.Web.Model;
using Aspnet.Web.BLL;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.BLL.Services;
using Aspnet.Web.Common;
using Microsoft.AspNet.Identity;

namespace Aspnet.Web.Sample.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            User.Identity.GetUserId<int>();
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                IUserService userService = new UserService();
                var user = userService.GetUser(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError(nameof(model.UserName), "用户名错误！");
                }
                else if (model.Password.MD5() != user.Password)
                {
                    ModelState.AddModelError(nameof(model.Password), "密码错误！");
                }
                else
                {
                    ToLogin(user);
                    userService.UpdateLastLoginTime(user.Id);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        private void ToLogin(User user)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.Nick, DateTime.Now, DateTime.Now.AddMinutes(30), false, user.Id.ToString(), "/");
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            HttpContext.Response.Cookies.Add(cookie);

        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegistryModel model)
        {
            if (ModelState.IsValid)
            {
                IUserService userService = new UserService();
                if (userService.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError(nameof(model.UserName), "用户名已存在！");
                }
                else
                {
                    User user = new User();
                    user.Name = model.UserName;
                    user.Nick = model.NickName;
                    user.Password = model.Password.MD5();
                    user.Admin = false;
                    user.CreateTime = DateTime.Now;
                    user.Id = userService.AddUser(user);
                    ToLogin(user);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }
    }
}