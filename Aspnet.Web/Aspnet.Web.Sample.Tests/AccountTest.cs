using System;
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
            var mock = new Mock<IUserService>();
            // Arrange
            AccountController controller = new AccountController(mock.Object);
            // Act
            ViewResult result = controller.Login() as ViewResult;
            // Assert
            Assert.AreEqual(result.ViewName, "");
            Assert.AreEqual(result.Model, null);
        }

        [TestMethod]
        public void LoginPost()
        {
            // Mock
            var mock = new Mock<IUserService>();
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
            mock.Setup(u => u.GetUser("test_user"))
                .Returns(fakeUser);
            mock.Setup(u => u.UpdateLastLoginTime(1))
                .Returns(true);

            // Arrange
            AccountController controller1 = new AccountController(mock.Object);
            // Act
            var testUser1 = new LoginModel()
            {
                UserName = "test_user",
                Password = "test_pwd1"
            };
            ViewResult result1 = controller1.Login(testUser1) as ViewResult;
            // Assert
            Assert.IsNotNull(result1);
            Assert.AreEqual(result1.ViewName, "");
            Assert.AreEqual(result1.ViewData.ModelState.IsValid, false);
            Assert.AreEqual(result1.Model, testUser1);

            // Arrange
            AccountController controller2 = new AccountController(mock.Object);
            // Act
            var testUser2 = new LoginModel()
            {
                UserName = "test_user",
                Password = "test_pwd"
            };
            RedirectToRouteResult result2 = controller2.Login(testUser2) as RedirectToRouteResult;
            // Assert
            Assert.IsNotNull(result2);
        }
    }
}
