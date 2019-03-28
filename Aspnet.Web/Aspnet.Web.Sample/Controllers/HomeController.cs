using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Aspnet.Web.Sample.Models;
using Microsoft.AspNet.Identity;

namespace Aspnet.Web.Sample.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

    }
}