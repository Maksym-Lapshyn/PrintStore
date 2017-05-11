using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PrintStore.Infrastructure.Concrete;
using PrintStore.Models;

namespace PrintStore.Infrastructure
{
    public class ActionLoggingAttribute : ActionFilterAttribute
    {
        public const string Anonymous = "anonymous";
        public const string Local = "local";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            EFLoggingLayer layer = new EFLoggingLayer();
            var request = filterContext.HttpContext.Request;
            ActionDetail actionDetail = new ActionDetail()
            {
                UserName = request.IsAuthenticated ? filterContext.HttpContext.User.Identity.Name : Anonymous,
                //HTTP_X_FORWARDED_FOR stands for HTTP header used by proxy servers
                IPAddress = request.IsLocal ? Local : request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress,
                Controller = request.RequestContext.RouteData.Values["action"].ToString(),
                Action = request.RequestContext.RouteData.Values["controller"].ToString(),
                RawURL = request.RawUrl,
                Date = DateTime.UtcNow
            };
            layer.LogAction(actionDetail);
            base.OnActionExecuting(filterContext);
        }
    }
}