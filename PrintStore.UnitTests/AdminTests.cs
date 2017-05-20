using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PrintStore.Domain.Infrastructure.Abstract;
using PrintStore.Domain.Entities;
using System.Web.Mvc;
using PrintStore.Controllers;
using PrintStore.Infrastructure.Abstract;
using System.Linq;
using PrintStore.Models;
using System.Web;

namespace PrintStore.UnitTests
{
    /// <summary>
    /// Tests actions of Admin Controller
    /// </summary>
    [TestClass]
    public class AdminTests
    {
        [TestMethod]
        public void GetCategories_Contains_All_Categories()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Categories).Returns(new List<Category>
            {
                new Category { CategoryId = 1, Name = "first" },
                new Category { CategoryId = 2, Name = "second" },
                new Category { CategoryId = 3, Name = "third" }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            List<Category> result = ((IEnumerable<Category>)target.GetCategories().ViewData.Model).ToList();
            //assert
            Assert.AreEqual(result[0].Name, "first");
            Assert.AreEqual(result[1].Name, "second");
            Assert.AreEqual(result[2].Name, "third");
        }

        [TestMethod]
        public void Categories_In_GetCategories_Contain_All_Products()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Categories).Returns(new List<Category>()
            {
                new Category { CategoryId = 1, Name = "first", Products = new List<Product>
                {
                    new Product { ProductId = 1, Name = "first" },
                    new Product { ProductId = 2, Name = "second" },
                    new Product { ProductId = 3, Name = "third" }
                } }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            List<Product> result = ((IEnumerable<Category>)target.GetCategories().ViewData.Model).First().Products.ToList();
            //assert
            Assert.AreEqual(result[0].Name, "first");
            Assert.AreEqual(result[1].Name, "second");
            Assert.AreEqual(result[2].Name, "third");
        }

        [TestMethod]
        public void Can_Edit_Categories()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Categories).Returns(new List<Category>()
            {
                new Category { CategoryId = 1, Name = "first" },
                new Category { CategoryId = 2, Name = "second" },
                new Category { CategoryId = 3, Name = "third" }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            Category c1 = target.EditCategory(1).ViewData.Model as Category;
            Category c2 = target.EditCategory(2).ViewData.Model as Category;
            Category c3 = target.EditCategory(3).ViewData.Model as Category;
            //assert
            Assert.AreEqual(c1.Name, "first");
            Assert.AreEqual(c2.Name, "second");
            Assert.AreEqual(c3.Name, "third");
        }

        [TestMethod]
        public void Can_Edit_Products()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>()
            {
                new Product { ProductId = 1, Name = "first" },
                new Product { ProductId = 2, Name = "second" },
                new Product { ProductId = 3, Name = "third" }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            Product p1 = target.EditProduct(1).ViewData.Model as Product;
            Product p2 = target.EditProduct(2).ViewData.Model as Product;
            Product p3 = target.EditProduct(3).ViewData.Model as Product;
            //assert
            Assert.AreEqual(p1.Name, "first");
            Assert.AreEqual(p2.Name, "second");
            Assert.AreEqual(p3.Name, "third");
        }

        [TestMethod]
        public void GetUsers_Contains_All_Users()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfUserLayer.Setup(m => m.Users).Returns(new List<ApplicationUser>
            {
                new ApplicationUser {IsBlocked = true },
                new ApplicationUser {IsBlocked = false },
                new ApplicationUser {IsBlocked = false }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            List<UserViewModel> result = ((IEnumerable<UserViewModel>)target.GetUsers().ViewData.Model).ToList();
            //assert
            Assert.IsTrue(result.Count == 3);
            Assert.AreEqual(true, result[0].IsBlocked);
            Assert.AreEqual(false, result[1].IsBlocked);
        }

        [TestMethod]
        public void GetOrders_Contains_All_Orders()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Orders).Returns(new List<Order>
            {
                new Order {OrderId = 1, TotalPrice = 500m },
                new Order {OrderId = 1, TotalPrice = 1000m },
                new Order {OrderId = 1, TotalPrice = 1500m },
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            List<Order> result = ((List<Order>)target.GetOrders().ViewData.Model).ToList();
            //assert
            Assert.IsTrue(result.Count == 3);
            Assert.IsTrue(result[0].TotalPrice == 500m);
        }

        [TestMethod]
        public void AddCategory_Creates_New_Category()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            Category result = target.AddCategory().ViewData.Model as Category;
            //assert
            Assert.IsTrue(result.CategoryId == 0);
            Assert.IsTrue(result.Name == null);
        }

        [TestMethod]
        public void AddProduct_Creates_New_Product()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            Product result = target.AddProduct(1).ViewData.Model as Product;
            //assert
            Assert.IsTrue(result.ProductId == 0);
            Assert.IsTrue(result.CategoryId == 1);
            Assert.IsTrue(result.Name == null);
        }

        [TestMethod]
        public void EditCategory_Redirects_Correctly()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            Category category = new Category() { CategoryId = 1, Name = "first" };
            //act
            RedirectToRouteResult result = (RedirectToRouteResult)target.EditCategory(category);
            //assert
            Assert.AreEqual("Admin", result.RouteValues["controller"]);
            Assert.AreEqual("GetCategories", result.RouteValues["action"]);
        }

        [TestMethod]
        public void DeleteCategory_Redirects_Correctly()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Categories).Returns(new List<Category>
            {
                new Category {CategoryId = 1, Name = "first" }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            RedirectToRouteResult result = (RedirectToRouteResult)target.DeleteCategory(1);
            //assert
            Assert.AreEqual("Admin", result.RouteValues["controller"]);
            Assert.AreEqual("GetCategories", result.RouteValues["action"]);
        }

        [TestMethod]
        public void EditProduct_Redirects_Correctly()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            Product product = new Product() { ProductId = 1, Name = "first" };
            //act
            RedirectToRouteResult result = (RedirectToRouteResult)target.EditProduct(product, null);
            //assert
            Assert.AreEqual("Admin", result.RouteValues["controller"]);
            Assert.AreEqual("GetCategories", result.RouteValues["action"]);
        }

        [TestMethod]
        public void DeleteProduct_Redirects_Correctly()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Name = "first" }
            });
            AdminController target = new AdminController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            //act
            RedirectToRouteResult result = (RedirectToRouteResult)target.DeleteProduct(1);
            //assert
            Assert.AreEqual("Admin", result.RouteValues["controller"]);
            Assert.AreEqual("GetCategories", result.RouteValues["action"]);
        }
    }
}
