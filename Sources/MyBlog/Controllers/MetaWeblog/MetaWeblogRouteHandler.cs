using System.Web;
using System.Web.Routing;

namespace MyBlog.Controllers.MetaWeblog
{
    /// <summary>
    /// Route Handler for MetaWebLog service
    /// </summary>
    internal sealed class MetaWeblogRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new MetaWeblogHandler();
        }
    }
}