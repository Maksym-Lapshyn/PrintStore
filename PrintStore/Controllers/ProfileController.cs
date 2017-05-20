using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Infrastructure.Abstract;
using Microsoft.AspNet.Identity;
using PrintStore.Domain.Entities;
using PrintStore.Infrastructure.Attributes;
using PrintStore.Models;

namespace PrintStore.Controllers
{
    /// <summary>
    /// Controller for product's functionality
    /// </summary>
    [ActionLogging]
    [ExceptionLogging]
    public class ProfileController : Controller
    {
        IBusinessLogicLayer businessLayer;

        //Contstuctor that takes argument from Ninject
        public ProfileController(IBusinessLogicLayer businessLayerParam)
        {
            businessLayer = businessLayerParam;
        }

        /// <summary>
        /// Dispays orders made by a user
        /// </summary>
        /// <returns>Partial view with view model of orders</returns>
        public ActionResult DisplayProfile()
        {
            string userId = User.Identity.GetUserId<string>();
            IEnumerable<Order> orders = businessLayer.Orders.Where(o => o.UserId == userId && o.IsDeleted == false).ToList();
            //Converts orders to view models of orders
            List<OrderViewModel> orderViewModels = orders.Select(o => new OrderViewModel()
            {
                DateAdded = o.DateAdded,
                OrderId = o.OrderId,
                TotalPrice = o.TotalPrice,
                OrderStatus = o.OrderStatus,
                CartLineViewModels = o.CartLines.Select(c => new CartLineViewModel() {
                    CartLineId = c.CartLineId,
                    ProductId = c.ProductId,
                    Quantity = c.Quantity,
                    Product = businessLayer.Products.Where(p => p.ProductId == c.ProductId).FirstOrDefault()
                }).ToList<CartLineViewModel>()
            }).ToList<OrderViewModel>();
            return PartialView(orderViewModels.ToList());
        }
    }
}