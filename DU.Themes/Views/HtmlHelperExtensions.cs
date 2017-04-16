using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DU.Themes.Views
{
    public static class HtmlHelperExtensions
    {
        public static string AddActiveClassIfRoutesMatch(this HtmlHelper helper, string action, string controller)
        {
            var ctrl = helper.ViewContext.RouteData.Values["controller"] as string;
            var actionName = helper.ViewContext.RouteData.Values["action"] as string;
            if (controller.Equals(ctrl, StringComparison.InvariantCultureIgnoreCase) && action.Equals(actionName, StringComparison.InvariantCultureIgnoreCase))
            {
                return "active";
            }

            return string.Empty;
            
        }
    }
}