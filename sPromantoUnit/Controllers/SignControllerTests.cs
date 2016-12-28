using Microsoft.VisualStudio.TestTools.UnitTesting;
using YPYA.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcContrib.TestHelper;
using System.Web.Mvc;

namespace YPYA.Controllers.Tests
{
    [TestClass()]
    public class SignControllerTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            //Arrange
            SignController signController = new SignController(); // you should mock your DbContext and pass that in

            // Act
            var result = signController.Login() as ViewResult;

            // Assert
            Assert.IsNotNull(result); // add additional checks on the Model
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Login");
        }

        [TestMethod()]
        public void RegisterTest()
        {
            //Arrange
            SignController signController = new SignController(); // you should mock your DbContext and pass that in

            // Act
            var result = signController.Register() as ViewResult;

            // Assert
            Assert.IsNotNull(result); // add additional checks on the Model
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName) || result.ViewName == "Register");
        }
        

        [TestMethod()]
        public void CikisYapTest()
        {
            var controller = new SignController();
            new TestControllerBuilder().InitializeController(controller);
            controller.HttpContext.Session["id"] = 1;

            controller.CikisYap();

            Assert.IsNull(controller.HttpContext.Session["id"]);
        }
    }
}