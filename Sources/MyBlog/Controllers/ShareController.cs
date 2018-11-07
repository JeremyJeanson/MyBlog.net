using MyBlog.Engine;
using MyBlog.Engine.Data.Models;
using MyBlog.Models;
using MyLib.Web.Filters;
using System;
using System.Web.Mvc;
using static MyLib.Web.Html.SocialExtensions;

namespace MyBlog.Controllers
{
    [XRobotsTagNoIndex]
    public class ShareController : Controller
    {
        #region Declarations

        private const String FaceBookFormat = "https://facebook.com/sharer.php?u={0}";
        private const String TwitterFormat = "https://twitter.com/intent/tweet?url={0}&text={1}";
        private const String LinkedInFormat = "http://www.linkedin.com/shareArticle?mini=true&url={0}&title={1}";
        private const String GooglePlusFormat = "https://plus.google.com/share?url={0}";
        private const String RedditForamt = "https://www.reddit.com/submit?url={0}";
        private const String PinterestFormat = "http://pinterest.com/pin/create/button/?url={0}&description={1}";

        private const String YahooFormat = "http://compose.mail.yahoo.com/?To=&Subject={1}&body={0}";
        private const String VkFormat = "'https://vkontakte.ru/share.php?url={0}&title={1}&noparse=true";

        //private const String ViadeoFormat = "http://www.viadeo.com/shareit/share/?url={0}&title={1}&urlaffiliate=32005&encoding=UTF-8";
        private const String ViadeoFormat = "http://www.viadeo.com/shareit/share/?url={0}&title={1}&encoding=UTF-8";
        private const String WhatsAppFormat = "whatsapp://send?text={0} {1}";

        private const String YammerFormat = "https://www.yammer.com/messages/new?login=true&trk_event=yammer_share&status={0}"; //"https://www.yammer.com/home/bookmarklet?bookmarklet_pop=1&u={0}&t={1}";

        #endregion

        // GET: Share
        public ActionResult Index(ShareRequest model)
        {
            // Try to know if the id is a integer or a string
            Int32 id;
            String title;
            String uri;
            String status;

            if (Int32.TryParse(model.Id, out id))
            {
                // Try to get the post
                PostLink post;
                using (var db = new DataService())
                {
                    post = db.GetPostLink(id);
                }
                if (post == null) return HttpNotFound();

                // Format data
                title = Uri.EscapeDataString(post.Title);
                uri = Uri.EscapeUriString(post.Url);
                status = Uri.EscapeDataString(post.Title + " " + post.Url);
            }
            else
            {
                // Test the id content
                if (String.IsNullOrEmpty(model.Id)) return HttpNotFound();
                title = null;
                uri = Uri.EscapeUriString(model.Id);
                status = uri;
            }

            // redirect to social page requested          
            switch (model.N)
            {
                case SocialnetWork.Facebook:
                    {
                        return Redirect(String.Format(FaceBookFormat, uri));
                    }
                case SocialnetWork.GooglePlus:
                    {
                        return Redirect(String.Format(GooglePlusFormat, uri));
                    }
                case SocialnetWork.LinkedIn:
                    {
                        return Redirect(String.Format(LinkedInFormat, uri, title));
                    }
                case SocialnetWork.Reddit:
                    {
                        return Redirect(String.Format(RedditForamt, uri));
                    }
                case SocialnetWork.Pinterest:
                    {
                        return Redirect(String.Format(PinterestFormat, uri, title));
                    }
                case SocialnetWork.Yahoo:
                    {
                        return Redirect(String.Format(YahooFormat, uri, title));
                    }
                case SocialnetWork.Vk:
                    {
                        return Redirect(String.Format(VkFormat, uri, title));
                    }
                case SocialnetWork.Viadeo:
                    {
                        return Redirect(String.Format(ViadeoFormat, uri, title));
                    }
                case SocialnetWork.Yammer:
                    {
                        return Redirect(String.Format(YammerFormat, status));
                    }
                case SocialnetWork.Twitter:
                default:
                    {
                        return Redirect(String.Format(TwitterFormat, uri, title));
                    }
            }
        }
    }
}