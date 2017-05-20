using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PrintStore.Infrastructure.Helpers
{
    /// <summary>
    /// Custom helpers
    /// </summary>
    public static class CustomHelpers
    {
        /// <summary>
        /// Displays price selected by user in filter fields
        /// </summary>
        /// <param name="helper">Item that calls helper</param>
        /// <param name="name">Name of field to store price</param>
        /// <param name="value">Price selected by user</param>
        /// <param name="defaultValue">Default price value of field</param>
        /// <returns></returns>
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