using MyBlog.Engine;
using MyLib.Web.SoeSiteMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MyBlog.Controllers
{
    public class SiteMapController : Controller
    {
        // GET: SiteMap
        public ActionResult Index()
        {
            SoeSiteMap sitemap = new SoeSiteMap(
                // Gt items
                GetImtes().ToArray());

            return new SoeSiteMapResult(sitemap);
        }

        /// <summary>
        /// Returnn thee sitemap items for soe
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SoeSiteMapItem> GetImtes()
        {
            String baseUri = Settings.Current.Url+"/";

            // Home page
            yield return new SoeSiteMapItem(baseUri) { ChangeFrequence = Frequence.Daily, Priority = 1 };

            // About
            yield return new SoeSiteMapItem(baseUri + "About/");

            // All posts
            using(var db = new DataService())
            {
                foreach(var link in db.GetAllPostLink())
                {
                    yield return new SoeSiteMapItem(link.Url) { Lastmodified = link.DatePublishedGmt, ChangeFrequence = Frequence.Yearly };
                }
            }
        }
    }
}