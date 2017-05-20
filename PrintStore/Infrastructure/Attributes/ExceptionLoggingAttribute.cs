using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Infrastructure.Concrete;
using PrintStore.Models;


namespace PrintStore.Infrastructure.Attributes
{
    /// <summary>
    /// Attribute for logging exception information
    /// </summary>
    public class ExceptionLoggingAttribute : HandleErrorAttribute
    {
        /// <summary>
        /// Logs exceptions using EFLoggingLayer
        /// </summary>
        /// <param name="filterContext">Context of filter</param>
        public override void OnException(ExceptionContext filterContext)
        {
            EFLoggingLayer layer = new EFLoggingLayer();
            ExceptionDetail exceptionDetail = new ExceptionDetail()
            {
                Message = filterContext.Exception.Message,
                StackTrace = filterContext.Exception.StackTrace,
                Controller = filterContext.RouteData.Values["controller"].ToString(),
                Action = filterContext.RouteData.Values["action"].ToString(),
                Date = DateTime.UtcNow
            };
            filterContext.ExceptionHandled = true;
            layer.LogException(exceptionDetail);
            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary {
                {"controller", "Exception" },
                {"action", "DisplayExceptionPage" }
            });
        }
    }
}