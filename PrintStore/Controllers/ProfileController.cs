using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Domain.Infrastructure.Abstract;
using Microsoft.AspNet.Identity;
using PrintStore.Domain.Entities;
using PrintStore.Infrastructure.Attributes;

namespace PrintStore.Controllers
{
    [ActionLogging]
    [ExceptionLogging]
    public class ProfileController : Controller
    {
        IBusinessLogicLayer businessLayer;

        public ProfileController(IBusinessLogicLayer businessLayerParam)
        {
            businessLayer = businessLayerParam;
        }

        public ActionResult DisplayProfile()
        {
            string userId = User.Identity.GetUserId<string>();
            IEnumerable<Order> orders = businessLayer.Orders.Where(o => o.UserId == userId && o.IsDeleted == false);
            return PartialView(orders.ToList());
        }
    }
}