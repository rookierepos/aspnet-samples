﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using Aspnet.Web.Sample.Models;
using Aspnet.Web.Model;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.Common;
using Microsoft.AspNet.Identity;

namespace Aspnet.Web.Sample.Controllers
{
    public class AccountController : Controller
    {
        private IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [CustomAuthorize]
        [HttpGet]
        public async Task<ActionResult> List(int pageIndex = 1, int pageSize = 10)
        {
            return View(await _accountService.AccountListAsync(pageIndex, pageSize));
        }

        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var (user, key, message) = await _accountService.LoginAsync(model.UserName, model.Password);
                if (user == null)
                {
                    if (key == "userName")
                    {
                        ModelState.AddModelError(nameof(model.UserName), "用户名错误！");
                    }
                    else if (key == "password")
                    {
                        ModelState.AddModelError(nameof(model.Password), "密码错误！");
                    }
                    else
                    {
                        ModelState.AddModelError("", message);
                    }
                }
                else
                {
                    HttpContext.SignIn(user.Nick, DateTime.Now.AddMinutes(30), true, user.Id.ToString().Encrypt());
                    if (!returnUrl.IsNullOrEmpty() && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegistryModel model)
        {
            if (ModelState.IsValid)
            {
                User user = new User();
                user.Name = model.UserName;
                user.Nick = model.NickName;
                user.Password = model.Password.MD5();
                user.Admin = false;
                user.CreatedTime = DateTime.Now;

                var (id, message) = await _accountService.RegisterAsync(user);
                if (id == 0)
                {
                    ModelState.AddModelError("", message);
                }
                else
                {
                    user.Id = id;
                    HttpContext.SignIn(user.Nick, DateTime.Now.AddMinutes(30), true, user.Id.ToString().Encrypt());
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }

        public ActionResult LogOut()
        {
            HttpContext.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}