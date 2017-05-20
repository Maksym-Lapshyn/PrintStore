using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Entities;
using PrintStore.Domain.Infrastructure.Abstract;
using PrintStore.Models;
using Microsoft.AspNet.Identity;
using PrintStore.Infrastructure.Abstract;
using PrintStore.Infrastructure.Attributes;

namespace PrintStore.Controllers
{
    /// <summary>
    /// Controller for cart's functionality
    /// </summary>
    [ActionLogging]
    [ExceptionLogging]
    public class CartController : Controller
    {
        //Contstuctor that takes arguments from Ninject
        IBusinessLogicLayer businessLayer;
        IUserLayer userLayer;

        public CartController(IBusinessLogicLayer businessLayerParam, IUserLayer userLayerParam)
        {
            businessLayer = businessLayerParam;
            userLayer = userLayerParam;
        }

        /// <summary>
        /// Partial view that displays summary of user's cart in layout
        /// </summary>
        /// <returns>View model of cart</returns>
        public PartialViewResult DisplayCartSummary()
        {
            CartViewModel cartViewModel = GetCart();
            return PartialView(cartViewModel);
        }

        /// <summary>
        /// Displays total information of cart
        /// </summary>
        /// <returns>View model of cart</returns>
        public ViewResult DisplayCart()
        {
            CartViewModel cartViewModel = GetCart();
            return View(cartViewModel);
        }

        [HttpPost]
        public ViewResult AddToCart(int productId, int quantity)
        {
            CartViewModel cartViewModel = GetCart();
            cartViewModel.AddCartLineViewModel(productId, quantity);
            return View("DisplayCart", cartViewModel);
        }

        [HttpPost]
        public ViewResult RemoveFromCart(int productId)
        {
            CartViewModel cartViewModel = GetCart();
            cartViewModel.RemoveCartLineViewModel(productId);
            Product product = businessLayer.Products.Where(p => p.ProductId == productId).First();
            TempData["message"] = string.Format("{0} was successfully removed from your cart", product.Name);
            return View("DisplayCart", cartViewModel);
        }

        /// <summary>
        /// Clears session, and converts cartlines of cart and id of user into order
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ViewResult CheckOut()
        {
            CartViewModel cartViewModel = GetCart();
            Session.Clear();
            string userId = User.Identity.GetUserId<string>();
            List<CartLine> cartLines = cartViewModel.CartLineViewModels.Select(c => new CartLine() { ProductId = c.ProductId, Quantity = c.Quantity, TotalPrice = c.TotalPrice }).ToList();
            businessLayer.SaveOrder(cartLines, userId);
            TempData["message"] = string.Format("Your order was successfully registered");
            return View("DisplayCart", GetCart());
        }

        /// <summary>
        /// Gets cart from session if it already stores a cart or creates a new cart and adds it to session
        /// </summary>
        /// <returns>View model for cart</returns>
        private CartViewModel GetCart()
        {
            CartViewModel cartViewModel = (CartViewModel)Session["cartViewModel"];
            if (cartViewModel == null)
            {
                cartViewModel = new CartViewModel(businessLayer);
                if (Request.IsAuthenticated)
                {
                    cartViewModel.UserId = User.Identity.GetUserId<string>();
                    cartViewModel.UserIsBlocked = userLayer.Users.Where(u => u.Id == cartViewModel.UserId).First().IsBlocked;
                }

                Session["cartViewModel"] = cartViewModel;
            }

            return cartViewModel;
        }
    }
}