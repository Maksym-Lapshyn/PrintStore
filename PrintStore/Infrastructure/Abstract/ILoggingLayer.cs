using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrintStore.Models;

namespace PrintStore.Infrastructure.Abstract
{
    public interface ILoggingLayer
    {
        void LogException(ExceptionDetail detail);

        void LogAction(ActionDetail detail);
    }
}
