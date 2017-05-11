using System;
using System.Text;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    /// <summary>
    /// Summary description for EntityProductRepositoryTests
    /// </summary>
    [TestClass]
    public class EntityProductRepositoryTests
    {
        [TestMethod]
        public void SaveProductChanges_CanSave()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            var controller = new AdminController(mock.Object);
            var product = new Product { Name = "Test" };
            
            //Act
            var result = controller.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()));
            Assert.IsNotInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void SaveProduct_InvalidChanges_CannotSave()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            var controller = new AdminController(mock.Object);
            var product = new Product {Name = "Test"};
            controller.ModelState.AddModelError("error", "error");

            //Act
            var result = controller.Edit(product);

            //Assert
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }
    }
}
