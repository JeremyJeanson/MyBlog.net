using MyBlog.Engine;
using MyLib.Web.Results;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class FeedController : Controller
    {
        public ActionResult Index()
        {
            return new FeedResult(FeedService.Get(), FeedResult.Type.Rss);
        }

        public ActionResult Atom()
        {
            return new FeedResult(FeedService.Get(), FeedResult.Type.Atom);
        }

        public ActionResult Rss()
        {
            return new FeedResult(FeedService.Get(), FeedResult.Type.Rss);
        }
    }
}