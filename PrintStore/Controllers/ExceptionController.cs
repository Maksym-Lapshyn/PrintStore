using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintStore.Controllers
{
    public class ExceptionController : Controller
    {
        public ActionResult DisplayExceptionPage()
        {
            return View();
        }
    }
}