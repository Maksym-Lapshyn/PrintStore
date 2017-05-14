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
    /// Summary description for CartTests
    /// </summary>
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void CartViewModel_Can_Add_CartLine()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Price = 50m, Name = "first" }
            });
            //act
            CartViewModel result = new CartViewModel(mockOfBusinessLogicLayer.Object);
            result.AddCartLineViewModel(1, 5);
            //assert
            Assert.IsTrue(result.CartLineViewModels.Count == 1);
            Assert.IsNotNull(result.CartLineViewModels.FirstOrDefault());
        }

        [TestMethod]
        public void CartViewModel_Calculates_TotalPrice()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Price = 50m, Name = "first" },
                new Product {ProductId = 2, Price = 100m, Name = "second" },
                new Product {ProductId = 3, Price = 200m, Name = "third" }
            });
            //act
            CartViewModel result = new CartViewModel(mockOfBusinessLogicLayer.Object);
            result.AddCartLineViewModel(1, 5);
            result.AddCartLineViewModel(2, 5);
            result.AddCartLineViewModel(3, 5);
            //assert
            Assert.IsTrue(result.TotalPrice == 1750m);
        }

        [TestMethod]
        public void CartViewModel_Removes_CartLineViewModels()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Product product = new Product { ProductId = 1, Price = 50m, Name = "first" };
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product> { product });
            //act
            CartViewModel result = new CartViewModel(mockOfBusinessLogicLayer.Object);
            result.CartLineViewModels = new List<CartLineViewModel> { new CartLineViewModel
            {
                CartLineId = 1, ProductId = 1, Product = product, Quantity = 5 }
            };
            result.RemoveCartLineViewModel(1);
            //assert
            Assert.IsTrue(result.CartLineViewModels.Count == 0);
        }

        [TestMethod]
        public void CartViewModel_Clears_CartLineViewModels()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            //act
            CartViewModel result = new CartViewModel(mockOfBusinessLogicLayer.Object);
            result.CartLineViewModels = new List<CartLineViewModel>
            {
                new CartLineViewModel{CartLineId = 1, ProductId = 1, Product = new Product { ProductId = 1, Price = 50m}, Quantity = 5},
                new CartLineViewModel{CartLineId = 2, ProductId = 2, Product = new Product { ProductId = 2, Price = 150m}, Quantity = 7},
                new CartLineViewModel{CartLineId = 3, ProductId = 3, Product = new Product { ProductId = 3, Price = 250m}, Quantity = 9}
            };
            result.Clear();
            //assert
            Assert.IsTrue(result.CartLineViewModels.Count == 0);
        }

        [TestMethod]
        public void DisplayCartSummary_Displays_Cart_Summary()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            Mock<ControllerContext> mockOfControllerContext = new Mock<ControllerContext>();
            mockOfControllerContext.SetupGet(m => m.HttpContext.Session["cartViewModel"]).Returns(new CartViewModel(mockOfBusinessLogicLayer.Object));
            CartController target = new CartController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            target.ControllerContext = mockOfControllerContext.Object;
            //act
            PartialViewResult result = target.DisplayCartSummary();
            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(CartViewModel));
        }

        [TestMethod]
        public void DisplayCart_Displays_Correct_Cart()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            Mock<ControllerContext> mockOfControllerContext = new Mock<ControllerContext>();
            mockOfControllerContext.SetupGet(m => m.HttpContext.Session["cartViewModel"]).Returns(new CartViewModel(mockOfBusinessLogicLayer.Object)
            {
                UserId = "firstUserId",
                UserIsBlocked = true
            });
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Price = 50m }
            });
            CartController target = new CartController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            target.ControllerContext = mockOfControllerContext.Object;
            //act
            CartViewModel result = (CartViewModel)target.DisplayCart().Model;
            //assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(CartViewModel));
            Assert.AreEqual("firstUserId", result.UserId);
        }

        [TestMethod]
        public void AddToCart_Adds_CartLine()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            Mock<ControllerContext> mockOfControllerContext = new Mock<ControllerContext>();
            mockOfControllerContext.SetupGet(m => m.HttpContext.Session["cartViewModel"]).Returns(new CartViewModel(mockOfBusinessLogicLayer.Object)
            {
                UserId = "firstUserId",
                UserIsBlocked = true,
                CartLineViewModels = new List<CartLineViewModel>()
            });
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Price = 50m }
            });
            CartController target = new CartController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            target.ControllerContext = mockOfControllerContext.Object;
            //act
            CartViewModel result = (CartViewModel)target.AddToCart(1, 5).Model;
            //assert
            Assert.IsInstanceOfType(result, typeof(CartViewModel));
            Assert.AreEqual("firstUserId", result.UserId);
            Assert.AreEqual(250m, result.TotalPrice);
        }

        [TestMethod]
        public void RemoveFromCart_Removes_CartLine()
        {
            //arrange
            Mock<IBusinessLogicLayer> mockOfBusinessLogicLayer = new Mock<IBusinessLogicLayer>();
            Mock<IUserLayer> mockOfUserLayer = new Mock<IUserLayer>();
            Mock<ControllerContext> mockOfControllerContext = new Mock<ControllerContext>();
            mockOfControllerContext.SetupGet(m => m.HttpContext.Session["cartViewModel"]).Returns(new CartViewModel(mockOfBusinessLogicLayer.Object)
            {
                UserId = "firstUserId",
                UserIsBlocked = true,
                CartLineViewModels = new List<CartLineViewModel>()
            });
            mockOfBusinessLogicLayer.Setup(m => m.Products).Returns(new List<Product>
            {
                new Product {ProductId = 1, Price = 50m }
            });
            CartController target = new CartController(mockOfBusinessLogicLayer.Object, mockOfUserLayer.Object);
            target.ControllerContext = mockOfControllerContext.Object;
            //act
            CartViewModel result = (CartViewModel)target.RemoveFromCart(1).Model;
            //assert
            Assert.IsInstanceOfType(result, typeof(CartViewModel));
            Assert.IsTrue(result.TotalPrice == default(decimal));
            Assert.IsTrue(result.CartLineViewModels.Count == 0);
        }
    }
}
