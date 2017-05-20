using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintStore.Controllers
{
    /// <summary>
    /// Controller to which application redirects in case of exception
    /// </summary>
    public class ExceptionController : Controller
    {
        //Default page of exception
        public ActionResult DisplayExceptionPage()
        {
            return View();
        }
    }
}