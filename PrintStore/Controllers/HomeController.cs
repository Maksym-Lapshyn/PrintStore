using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Infrastructure.Attributes;

namespace PrintStore.Controllers
{
    /// <summary>
    /// Controller for index page
    /// </summary>
    [ActionLogging]
    [ExceptionLogging]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}