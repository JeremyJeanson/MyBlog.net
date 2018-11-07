using MyBlog.Controllers.MetaWeblog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MyBlog
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // Route XMRPC for MetaWeblog
            routes.Add(
                MetaWeblogConfiguration.XmlRpcControllerName,
                new Route(
                    MetaWeblogConfiguration.XmlRpcControllerName,
                    new RouteValueDictionary { { "controller", MetaWeblogConfiguration.XmlRpcControllerName } },
                    new MetaWeblogRouteHandler()));            

            routes.MapRoute(
                 "Default",
                 "{controller}/{action}/{id}",
                 new { controller = "Post", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                "WithTitle",
                "Post/Details/{id}/{title}",
                new { controller = "Post", action = "Details", title = UrlParameter.Optional }
            );
        }
    }
}
