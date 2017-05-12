using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Infrastructure.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void Login_ValidCredentials_CanLogin()
        {
            //Arange
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("admin", "admin")).Returns(true);
            
            var loginViewModel = new LoginViewModel
            {
                UserName = "admin",
                Password = "admin"
            };

            var controller = new AccountController(mock.Object);

            //Act
            var result = controller.Login(loginViewModel, "/MyURL");

            //Assert
            Assert.IsInstanceOfType(result, typeof(RedirectResult));
            Assert.AreEqual("/MyURL", ((RedirectResult)result).Url);
        }

        [TestMethod]
        public void Login_InvalidCredentials_CannotLogin()
        {
            //Arange
            var mock = new Mock<IAuthProvider>();
            mock.Setup(m => m.Authenticate("user", "crack")).Returns(false);

            var loginViewModel = new LoginViewModel
            {
                UserName = "user",
                Password = "crack"
            };

            var controller = new AccountController(mock.Object);

            //Act
            var result = controller.Login(loginViewModel, "/MyURL");

            //Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
            Assert.IsFalse(((ViewResult)result).ViewData.ModelState.IsValid);
        }
    }
}
