using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKW.AdminPortal.Extensions
{
    public static class HtmlHelperExtension
    {
        public static string IsSelected(this IHtmlHelper html, string page=null, string area = null)
        {
            string cssClass = "active";
            string currentPage = (string)html.ViewContext.RouteData.Values["page"];
            string currentArea = (string)html.ViewContext.RouteData.Values["area"];
           
            if (string.IsNullOrEmpty(page))
                page = currentPage;

            if (string.IsNullOrEmpty(area))
                area = currentArea;

            return page == currentPage && area == currentArea ?
                cssClass : string.Empty;
        }
    }
}
