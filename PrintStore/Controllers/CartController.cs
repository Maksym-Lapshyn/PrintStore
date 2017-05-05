using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Entities;
using PrintStore.Domain.Concrete;
using Microsoft.AspNet.Identity;

namespace PrintStore.Controllers
{
    public class CartController : Controller
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();

        public ActionResult DisplayCartSummary()
        {
            Cart cart = GetCart();
            return PartialView(cart);
        }

        public ActionResult DisplayCart()
        {
            Cart cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        public ActionResult AddToCart(int productId, int quantity)
        {
            Cart cart = GetCart();
            cart.AddCartLine(productId, quantity);
            return View("DisplayCart", cart);
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            Cart cart = GetCart();
            cart.RemoveCartLine(productId);
            Product product = layer.Products.Where(p => p.ProductId == productId).First();
            TempData["message"] = string.Format("{0} was successfully removed from your cart", product.Name);
            return View("DisplayCart", cart);
        }

        [HttpPost]
        public ActionResult CheckOut()
        {
            Cart cart = GetCart();
            Session.Clear();
            string userId = User.Identity.GetUserId<string>();
            layer.SaveOrder(cart, userId);
            TempData["message"] = string.Format("Your order was successfully registered");
            return View("DisplayCart", GetCart());
        }

        private Cart GetCart()
        {
            Cart cart = (Cart)Session["cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["cart"] = cart;
            }

            return cart;
        }
    }
}