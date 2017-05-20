using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrintStore.Models
{
    /// <summary>
    /// Information for logging actions
    /// </summary>
    public class ActionDetail
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string IPAddress { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string RawURL { get; set; }
        public DateTime Date { get; set; }
    }
}