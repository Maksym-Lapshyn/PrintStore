using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Entities;
using PrintStore.Domain.Infrastructure.Concrete;
using PrintStore.Models;
using Microsoft.AspNet.Identity;
using PrintStore.Infrastructure.Concrete;
using PrintStore.Infrastructure.Attributes;

namespace PrintStore.Controllers
{
    [ActionLogging]

    public class CartController : Controller
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();
        IdentityUserLayer userLayer = new IdentityUserLayer();

        public ActionResult DisplayCartSummary()
        {
            CartViewModel cartViewModel = GetCart();
            return PartialView(cartViewModel);
        }

        public ActionResult DisplayCart()
        {
            CartViewModel cartViewModel = GetCart();
            return View(cartViewModel);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            CartViewModel cartViewModel = GetCart();
            cartViewModel.AddCartLineViewModel(productId, quantity);
            return View("DisplayCart", cartViewModel);
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            CartViewModel cartViewModel = GetCart();
            cartViewModel.RemoveCartLineViewModel(productId);
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            TempData["message"] = string.Format("{0} was successfully removed from your cart", product.Name);
            return View("DisplayCart", cartViewModel);
        }

        [HttpPost]
        public ActionResult CheckOut()
        {
            CartViewModel cartViewModel = GetCart();
            Session.Clear();
            string userId = User.Identity.GetUserId<string>();
            List<CartLine> cartLines = cartViewModel.CartLineViewModels.Select(c => new CartLine() { ProductId = c.ProductId, Quantity = c.Quantity, TotalPrice = c.TotalPrice }).ToList();
            layer.SaveOrder(cartLines, userId);
            TempData["message"] = string.Format("Your order was successfully registered");
            return View("DisplayCart", GetCart());
        }

        private CartViewModel GetCart()
        {
            CartViewModel cartViewModel = (CartViewModel)Session["cartViewModel"];
            if (cartViewModel == null)
            {
                cartViewModel = new CartViewModel();
                cartViewModel.UserId = User.Identity.GetUserId<string>();
                if (Request.IsAuthenticated)
                {
                    cartViewModel.UserIsBlocked = userLayer.Users.Where(u => u.Id == cartViewModel.UserId).First().IsBlocked;
                }

                Session["cartViewModel"] = cartViewModel;
            }

            return cartViewModel;
        }
    }
}