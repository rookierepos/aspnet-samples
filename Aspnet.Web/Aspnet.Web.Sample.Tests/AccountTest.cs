using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Aspnet.Web.BLL.Abstractions;
using Aspnet.Web.Common;
using Aspnet.Web.Model;
using Aspnet.Web.Sample.Controllers;
using Aspnet.Web.Sample.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Aspnet.Web.Sample.Tests
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void LoginGet()
        {
            // Mock
            var mock = new Mock<IAccountService>();
            // Arrange
            AccountController controller = new AccountController(mock.Object);
            // Act
            ViewResult result = controller.Login("") as ViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "");
            Assert.AreEqual(result.Model, null);
        }

        [TestMethod]
        public async Task LoginPost()
        {
            // Mock
            var mock = new Mock<IAccountService>();
            var fakeUser = new User()
            {
                Id = 1,
                Name = "test_user",
                Nick = "test_nick",
                Admin = false,
                CreateTime = DateTime.Today,
                LastLoginTime = DateTime.Today,
                LockedDate = null,
                Password = "test_pwd".MD5(),
                Status = UserStatus.Normal
            };
            mock.Setup((accountService) => accountService.LoginAsync("test_user", "test_pwd1"))
                .Returns(Task.FromResult(((User)null, "userName", "用户名错误！")));

            // Arrange
            AccountController controller1 = new AccountController(mock.Object);
            // Act
            var testUser1 = new LoginModel()
            {
                UserName = "test_user",
                Password = "test_pwd1"
            };
            ViewResult result1 = await controller1.Login(testUser1, "") as ViewResult;
            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(result1.ViewName, "");
            Assert.AreEqual(result1.ViewData.ModelState.IsValid, false);
            Assert.AreEqual(result1.Model, testUser1);
            
            mock.Setup((accountService) => accountService.LoginAsync("test_user", "test_pwd"))
                .Returns(Task.FromResult((fakeUser, "", "")));

            // Arrange
            AccountController controller2 = new AccountController(mock.Object);
            // Act
            var testUser2 = new LoginModel()
            {
                UserName = "test_user",
                Password = "test_pwd"
            };
            RedirectToRouteResult result2 = await controller2.Login(testUser2, "") as RedirectToRouteResult;
            // Assert
            Assert.IsNotNull(result2.RouteValues["action"].ToString() == "Index");
            Assert.IsNotNull(result2.RouteValues["controller"].ToString() == "Home");
        }

        [TestMethod]
        public async Task ListGet()
        {
            // Mock
            var mock = new Mock<IAccountService>();
            IEnumerable<User> list = new List<User>()
            {
                new User
                {
                    Id = 1,
                    Name = "test_user",
                    Nick = "test_nick",
                    Admin = false,
                    CreateTime = DateTime.Today,
                    LastLoginTime = DateTime.Today,
                    LockedDate = null,
                    Password = "test_pwd".MD5(),
                    Status = UserStatus.Normal
                }
            };
            mock.Setup((accountService) => accountService.AccountListAsync(1, 10))
                .Returns(Task.FromResult(list));
            
            // Arrange
            AccountController controller = new AccountController(mock.Object);

            ViewResult result1 = await controller.List(1, 10) as ViewResult;

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual((result1.Model as IEnumerable<User>)?.Count() ?? 0, 1);

            ViewResult result2 = await controller.List(2, 10) as ViewResult;

            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual((result1.Model as IEnumerable<User>)?.Count() ?? 0, 0);

        }
    }
}
