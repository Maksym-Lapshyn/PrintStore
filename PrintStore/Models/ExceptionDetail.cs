using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintStore.Models
{
    public class ExceptionDetail
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string StackTrace { get; set; }
        public DateTime Date { get; set; }
    }
}