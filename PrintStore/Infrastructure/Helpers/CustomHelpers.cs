using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintStore.Infrastructure.Helpers
{
    public static class CustomHelpers
    {
        public static MvcHtmlString SelectedPriceHelper(this HtmlHelper helper, string name, decimal value, decimal defaultValue)
        {
            TagBuilder input = new TagBuilder("input");
            input.AddCssClass("form-control");
            input.Attributes.Add("name", name);
            input.Attributes.Add("type", "text");
            if (value != default(Decimal))
            {
                input.Attributes.Add("value", value.ToString());
            }
            else
            {
                input.Attributes.Add("value", defaultValue.ToString());
            }

            return new MvcHtmlString(input.ToString());
        }
    }
}