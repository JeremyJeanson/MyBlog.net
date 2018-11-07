using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace MyLib.Web.SoeSiteMap
{
    public sealed class SoeSiteMapResult : ActionResult
    {
        private readonly SoeSiteMap _sitemap;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sitemap"></param>
        public SoeSiteMapResult(SoeSiteMap sitemap)
        {
            _sitemap = sitemap;
        }

        /// <summary>
        /// Execute Result
        /// </summary>
        /// <param name="context"></param>
        public override void ExecuteResult(ControllerContext context)
        {
            // Get the response
            HttpResponseBase response = context.HttpContext.Response;

            // Add header en encoding
            response.ContentEncoding = Encoding.UTF8;
            response.ContentType = "text/xml";
            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false)
            };
            using (XmlWriter writer = XmlWriter.Create(response.Output, settings))
            {
                XDocument doc = _sitemap.GetXDocument();
                doc.WriteTo(writer);
            }
        }
    }
}
