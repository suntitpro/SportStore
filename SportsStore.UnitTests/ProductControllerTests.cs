using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ProductControllerTests
    {
        [TestMethod]
        public void GetImage_ValidImage_CanRetrieve()
        {
            //Arrange
            var prod = new Product
            {
                Id = 2,
                Name = "Test",
                ImageData = new byte[] {},
                ImageMimeType = "image/png"
            };

            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                prod,
                new Product {Id = 3, Name = "P3"}
            }.AsQueryable);

            var controller = new ProductController(mock.Object);
            
            //Act
            var result = controller.GetImage(2);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void GetImage_NotEWxistentImage_CannotRetrieve()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 3, Name = "P3"}
            }.AsQueryable);

            var controller = new ProductController(mock.Object);

            //Act
            var result = controller.GetImage(2);

            //Assert
            Assert.IsNull(result);
        }
    }
}
