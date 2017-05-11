using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PrintStore.Infrastructure.Contexts;
using PrintStore.Infrastructure.Abstract;
using PrintStore.Models;

namespace PrintStore.Infrastructure.Concrete
{
    public class EFLoggingLayer : ILoggingLayer
    {
        EFLoggingContext context = new EFLoggingContext();

        public void LogException(ExceptionDetail detail)
        {
            context.ExceptionDetails.Add(detail);
            context.SaveChanges();
        }

        public void LogAction(ActionDetail detail)
        {
            context.ActionDetails.Add(detail);
            context.SaveChanges();
        }
    }
}