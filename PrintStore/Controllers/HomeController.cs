using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Infrastructure;

namespace PrintStore.Controllers
{
    [ActionLogging]
    [ExceptionLogging]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}