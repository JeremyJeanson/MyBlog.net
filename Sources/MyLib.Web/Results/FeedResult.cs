using System;
using System.ServiceModel.Syndication;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace MyLib.Web.Results
{
    public sealed class FeedResult : ActionResult
    {
        #region Declarations

        private const String AtomContentType = "application/atom+xml";
        private const String RssContentType= "application/rss+xml";

        public enum Type
        {            
            Atom,
            Rss
        }

        private readonly SyndicationFeed _feed;
        private readonly Type _type;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="feed"></param>
        public FeedResult(SyndicationFeed feed, Type type)
        {
            _feed = feed;
            _type = type;
        }

        #endregion

        #region Methodes

        public override void ExecuteResult(ControllerContext context)
        {
            // Get the response
            HttpResponseBase response = context.HttpContext.Response;

            // Add header en encoding
            response.ContentEncoding = Encoding.UTF8;

            var settings = new XmlWriterSettings
            {
                Encoding = new UTF8Encoding(false)
            };
            using (XmlWriter writer = XmlWriter.Create(response.Output, settings))
            {
                if (_type==Type.Atom)
                {
                    Atom10FeedFormatter atomformatter = new Atom10FeedFormatter(_feed);
                    atomformatter.WriteTo(writer);
                }
                else
                {
                    Rss20FeedFormatter rssformatter = new Rss20FeedFormatter(_feed);
                    rssformatter.WriteTo(writer);
                }
            }
        }

        #endregion
    }
}
