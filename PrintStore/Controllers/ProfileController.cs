using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Concrete;
using Microsoft.AspNet.Identity;
using PrintStore.Domain.Entities;
using PrintStore.Infrastructure;

namespace PrintStore.Controllers
{
    [ActionLogging]
    [ExceptionLogging]
    public class ProfileController : Controller
    {
        EFBusinessLogicLayer layer = new EFBusinessLogicLayer();

        public ActionResult DisplayProfile()
        {
            string userId = User.Identity.GetUserId<string>();
            IEnumerable<Order> orders = layer.Orders.Where(o => o.UserId == userId && o.IsDeleted == false);
            return PartialView(orders.ToList());
        }
    }
}