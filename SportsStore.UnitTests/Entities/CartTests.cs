using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests.Entities
{
    [TestClass()]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            //Arrange - create some test products
            var product1 = new Product { Id = 1, Name = "P1" };
            var product2 = new Product { Id = 2, Name = "P2" };

            //Arrange - create new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            var result = target.Details.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Product, product1);
            Assert.AreEqual(result[1].Product, product2);

        }

        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            //Arrange - create some test products
            var product1 = new Product { Id = 1, Name = "P1" };
            var product2 = new Product { Id = 2, Name = "P2" };

            //Arrange - create new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            target.AddItem(product2, 10);
            var result = target.Details.ToArray();

            //Assert
            Assert.AreEqual(result.Length, 2);
            Assert.AreEqual(result[0].Quantity, 1);
            Assert.AreEqual(result[1].Quantity, 11);
        }

        [TestMethod]
        public void Can_Remove_Line()
        {
            //Arrange - create some test products
            var product1 = new Product { Id = 1, Name = "P1" };
            var product2 = new Product { Id = 2, Name = "P2" };
            var product3 = new Product { Id = 3, Name = "P3" };

            //Arrange - create new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(product1, 1);
            target.AddItem(product2, 3);
            target.AddItem(product3, 5);
            target.AddItem(product2, 2);

            //Act
            target.RemoveDetail(product2);

            //Assert
            Assert.AreEqual(target.Details.Count(c => c.Product == product2), 0);
            Assert.AreEqual(target.Details.Count(), 2);
        }

        [TestMethod()]
        public void Calculate_Cart_Total()
        {
            //Arrange - create some test products
            var product1 = new Product { Id = 1, Name = "P1", Price = 100};
            var product2 = new Product { Id = 2, Name = "P2", Price = 50};
            
            //Arrange - create new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            target.AddItem(product2, 2);

            //Act
            var total = target.CalculateTotalValue();

            //Assert
            Assert.AreEqual(total, 250);
        }

        [TestMethod()]
        public void Can_Clear_Contents()
        {
            //Arrange - create some test products
            var product1 = new Product { Id = 1, Name = "P1", Price = 100 };
            var product2 = new Product { Id = 2, Name = "P2", Price = 50 };

            //Arrange - create new cart
            var target = new Cart();

            // Arrange - add some products to the cart
            target.AddItem(product1, 1);
            target.AddItem(product2, 1);
            
            //Act
            target.Clear();

            //Assert
            Assert.AreEqual(target.Details.Count(), 0);
        }

        [TestMethod]
        public void Can_Add_To_Cart()
        {
            //Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"}
            }.AsQueryable());

            //Arrange - create cart
            var cart = new Cart();

            //Arrange - create controller
            var controller = new CartController(mock.Object);

            //Act - add a product to cart
            controller.AddToCart(cart, 1, null);

            //Assert
            Assert.AreEqual(cart.Details.Count(), 1);
            Assert.AreEqual(cart.Details.ToArray()[0].Product.Id, 1);
        }

        [TestMethod]
        public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
        {
            //Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"}
            }.AsQueryable());

            //Arrange - create cart
            var cart = new Cart();

            //Arrange - create controller
            var controller = new CartController(mock.Object);

            //Act
            var result = controller.AddToCart(cart, 2, "myUrl");

            //Assert
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        [TestMethod]
        public void Can_View_Cart_Controller()
        {
            //Arrange - create cart
            var cart = new Cart();

            //Arrange - create controller
            var controller = new CartController(null);

            //Act
            var resutl = (CartIndexViewModel) controller.Index(cart, "myUrl").ViewData.Model;

            //Assert
            Assert.AreEqual(resutl.Cart, cart);
            Assert.AreEqual(resutl.ReturnUrl, "myUrl");
        }
    }
}