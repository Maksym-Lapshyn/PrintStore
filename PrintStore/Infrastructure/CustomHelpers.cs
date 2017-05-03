using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintStore.Infrastructure
{
    public static class CustomHelpers
    {
        public static MvcHtmlString CategoryDropDownList(this HtmlHelper helper, string name, Dictionary<int, string> categories, Object htmlAttributes)
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Selected = true, Text = "All", Value = "All" });
            foreach (var category in categories)
            {
                items.Add(new SelectListItem() { Text = category.Value, Value = category.Key.ToString() });
            }
            return System.Web.Mvc.Html.SelectExtensions.DropDownList(helper, name, items, htmlAttributes);
        }
    }
}