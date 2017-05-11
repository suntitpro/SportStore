using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void Index_Contains_AllProducts()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2" },
                new Product {Id = 3, Name = "P3"},
                new Product {Id = 4, Name = "P4"},
                new Product {Id = 5, Name = "P5"}
            });

            var controller = new AdminController(mock.Object);

            //Act
            var result = (IEnumerable<Product>)controller.Index().ViewData.Model;

            //Assert
            var prodArray = result.ToArray();
            Assert.IsTrue(prodArray.Length == 5);
            Assert.AreEqual(prodArray[0].Name, "P1");
            Assert.AreEqual(prodArray[1].Name, "P2");
            Assert.AreEqual(prodArray[4].Name, "P5");
        }

        [TestMethod]
        public void Edit_ExistentProduct_Valid()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2" },
                new Product {Id = 3, Name = "P3"}
            });

            var controller = new AdminController(mock.Object);

            //Act
            var product1 = controller.Edit(1).ViewData.Model as Product;
            var product2 = controller.Edit(2).ViewData.Model as Product;
            var product3 = controller.Edit(3).ViewData.Model as Product;


            //Assert
            Assert.AreEqual(1, product1.Id);
            Assert.AreEqual(2, product2.Id);
            Assert.AreEqual(3, product3.Id);
        }

        [TestMethod]
        public void Edit_NonExistentProduct_Cannot()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2" },
                new Product {Id = 3, Name = "P3"}
            });

            var controller = new AdminController(mock.Object);

            //Act
            var product1 = controller.Edit(7).ViewData.Model as Product;
            
            //Assert
            Assert.IsNull(product1);
        }

        [TestMethod]
        public void Delete_ValidProduct_Can()
        {
            //Arrange
            var prod = new Product {Id = 2, Name = "Test"};
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                prod,
                new Product {Id = 3, Name = "P3"}
            });

            var controller = new AdminController(mock.Object);

            //Act
            controller.Delete(prod.Id);

            //Assert
            mock.Verify(m => m.DeleteProduct(prod.Id));
        }
    }
}
