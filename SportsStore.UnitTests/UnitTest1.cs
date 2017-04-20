using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using SportsStore.WebUI.Models;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
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

            var controller = new ProductController(mock.Object) {PageSize = 3};

            //Act
            var result = (ProductsListViewModel) controller.List(null, 2).Model;

            //Assert
            var prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Arrange - define an HTML helper - we need to do this
            // in order to apply the extension method
            HtmlHelper myHelper = null;

            // Arrange - create PagingInfo data
            var pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Arrange - set up the delegate using a lambda expression
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            //Act
            var result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            //Assert
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>" 
                            + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" 
                            + @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1"},
                new Product {Id = 2, Name = "P2"},
                new Product {Id = 3, Name = "P3"},
                new Product {Id = 4, Name = "P4"},
                new Product {Id = 5, Name = "P5"}
            });

            //Arrange controller
            var controller = new ProductController(mock.Object) { PageSize = 3 };

            //Act
            var result = (ProductsListViewModel)controller.List(null, 2).Model;

            //Assert
            var pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }

        [TestMethod]
        public void Can_Filter_Products()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Cat1"},
                new Product {Id = 2, Name = "P2", Category = "Cat1" },
                new Product {Id = 3, Name = "P3", Category = "Cat2"},
                new Product {Id = 4, Name = "P4", Category = "Cat2"},
                new Product {Id = 5, Name = "P5", Category = "Cat3"}
            });

            //Arrange controller
            var controller = new ProductController(mock.Object) { PageSize = 3 };

            //Act
            var result = (ProductsListViewModel)controller.List("Cat2", 1).Model;

            //Assert
            var prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.IsTrue(prodArray[0].Name == "P3" && prodArray[0].Category == "Cat2");
            Assert.IsTrue(prodArray[1].Name == "P4" && prodArray[0].Category == "Cat2");
        }

        [TestMethod]
        public void Can_Create_Categories()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Cat3"},
                new Product {Id = 2, Name = "P2", Category = "Cat1" },
                new Product {Id = 3, Name = "P3", Category = "Cat2"},
                new Product {Id = 4, Name = "P4", Category = "Cat2"},
                new Product {Id = 5, Name = "P5", Category = "Cat3"}
            });

            //Arrange controller
            var navController = new NavController(mock.Object);

            //Act
            var results = (IEnumerable<string>) navController.Menu().Model;

            //Assert
            var resultsArray = results.ToArray();
            Assert.AreEqual(resultsArray.Length, 3);
            Assert.AreEqual(resultsArray[0], "Cat1");
            Assert.AreEqual(resultsArray[1], "Cat2");
            Assert.AreEqual(resultsArray[2], "Cat3");
        }

        [TestMethod]
        public void Indicates_Selected_Category()
        {
            //Arrange
            var mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]
            {
                new Product {Id = 1, Name = "P1", Category = "Cat3"},
                new Product {Id = 2, Name = "P2", Category = "Cat1" }
            });

            //Arrange controller
            var navController = new NavController(mock.Object);

            var categoryToSelect = "Cat1";
            
            //Action
            string result = navController.Menu(categoryToSelect).ViewBag.SelectedCategory;

            //Assert
            Assert.AreEqual(categoryToSelect, result);
        }
    }
}
