using MyLib.Web.Strings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyLib.Web.Html
{
    public static class PaginationExtensions
    {
        private const Int32 PageMax = 10;

        public static MvcHtmlString Pagination(this HtmlHelper htmlHelper, String action, Int32 count, Int32 page, Int32 pageSize)
        {
            return Pagination(htmlHelper, action, count, page, pageSize, null);
        }

        public static MvcHtmlString Pagination(this HtmlHelper htmlHelper, String action, Int32 count, Int32 page, Int32 pageSize, Object routeValues)
        {
            // Args
            IDictionary<String, Object> args = (routeValues as IDictionary<String, Object> ?? new RouteValueDictionary(routeValues));

            // Create a nav container
            TagBuilder nav = new TagBuilder("nav");
            nav.MergeAttribute("aria-label", Resources.Pagination);

            // Add the ul
            TagBuilder ul = new TagBuilder("ul");
            ul.AddCssClass("pagination");

            // Get the current UrlHelper to creat links
            var urlHelper = new System.Web.Mvc.UrlHelper(htmlHelper.ViewContext.RequestContext);
            StringBuilder sb = new StringBuilder();

            // Add li for each page
            Int32[] pages = Enumerable.Range(page - pageSize, PageMax)
                .Where(c => c >= 0 && c * pageSize < count)
                .ToArray();

            // Setting fo li list
            Int32 length = pages.Length;

            // Nothing to write if we have only one page or less
            if (length <= 1) return null;


            Int32 min = pages[0];
            Int32 max = pages[length - 1];
            Boolean haveMore = (max+1) * pageSize < count;

            // General li list
            for (Int32 i = 0; i < length; i++)
            {
                // Create li
                TagBuilder li = new TagBuilder("li");
                // Page item style
                li.AddCssClass("page-item");
                // Test active page
                if (page == pages[i])
                {
                    li.AddCssClass("active");
                    // Inner span
                    TagBuilder span = new TagBuilder("span");
                    span.AddCssClass("page-link");
                    Int32 indexDesplayed = pages[i] + 1;
                    
                    // content in span
                    span.InnerHtml = (indexDesplayed).ToString();

                    // Screen reader alt text
                    TagBuilder spansr = new TagBuilder("span");
                    spansr.AddCssClass("sr-only");
                    spansr.InnerHtml = Resources.PageCurrent;

                    li.InnerHtml = span.ToString() + spansr.ToString();
                }
                else
                {
                    // Create a
                    TagBuilder a = new TagBuilder("a");
                    a.MergeAttribute("href", GetUrl(urlHelper, action, pages[i], args));
                    a.AddCssClass("page-link");
                    if (pages[i] == min && pages[i] != 0)
                    {
                        a.InnerHtml = "<span aria-hidden=\"true\">&laquo;</span>";
                        a.MergeAttribute("aria-label", Resources.PaginationPrevious);
                    }
                    else if (haveMore && pages[i] == max)
                    {
                        a.InnerHtml = "<span aria-hidden=\"true\">&raquo;</span>";
                        a.MergeAttribute("aria-label", Resources.PaginationNext);
                    }
                    else
                    {
                        Int32 indexDesplayed = pages[i] + 1;
                        a.InnerHtml = (indexDesplayed).ToString();
                        a.MergeAttribute("aria-label", Resources.PaginationIndex + " " + indexDesplayed);
                    }
                    li.InnerHtml = a.ToString();
                }

                // Append contents
                sb.Append(li.ToString());
            }

            // Ad li list to ul
            ul.InnerHtml = sb.ToString();

            // Add ul content to nav
            nav.InnerHtml = ul.ToString();

            // Return nav
            return new MvcHtmlString(nav.ToString());
        }

        private static String GetUrl(System.Web.Mvc.UrlHelper urlHelper,String action, Int32 page, IDictionary<String, Object> args)
        {
            var routeValues = new RouteValueDictionary(args);
            routeValues.Add("page", page);
            return urlHelper.Action(action, routeValues);
        }
    }
}
